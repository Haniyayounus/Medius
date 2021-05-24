using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.ViewModels;
using Application.Utils;
using Core;
using Core.Entities;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Application.Repository
{
    public sealed class UserService
    {
        private readonly MediusContext _dbContext;
        private readonly IHostingEnvironment _env;
        private bool _disposed;
        public UserService(MediusContext dbContext, IHostingEnvironment env)
        {
            this._dbContext = dbContext;
            this._env = env;
        }
        public UserService()
        {
            Dispose(false);
        }

        private void Dispose(bool val)
        {
            if (!_disposed)
            {
                if (val)
                {
                    // called via myClass.Dispose(). 
                    // OK to use any private object references
                }
                // Release unmanaged resources.
                // Set large fields to null.                
                _disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //list of users
        public async Task<List<User>> GetAllAppUsers()
        {
            return await _dbContext.Users.Where(x => x.UserRole.Role == UserType.AppUser && x.IsActive).AsNoTracking().ToListAsync();
        }

        //profile of user
        public async Task<User> GetUser(Guid id)
        {
            return await _dbContext.Users.Where(x => x.Id == id && x.IsActive).AsNoTracking().FirstOrDefaultAsync();
        }

        //sign up for user
        public async Task<User> SignUp(UserModel userModel)
        {
            User user = new User
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Contact = userModel.Contact,
                Cnic = userModel.Cnic,
                Email = userModel.Email,
                Password = userModel.Password,
                ImagePath = "Placeholder.jpg",
                RoleId = 1,
                CreatedAt = DateTime.Now,
                LastModify = DateTime.Now,
                IsActive = true,
                ResetCode = Convert.ToInt64("")
            };
            var emailExist = await ExitingEmail(user.Email);
            var usernameExist = await ExistingUsername(user.FirstName, user.LastName);
            var contactExist = await ExistingContact(user.Contact);
            
            if (emailExist || usernameExist || contactExist)
            {
                return user = null;
            }
            else
            {
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
                return user;
            }
        }

        //Update User Profile
        public async Task<User> ModifyProfile(UserModel userModel)
        {
            var user = await _dbContext.Users.Where(x => x.Id == userModel.Id && x.IsActive).FirstOrDefaultAsync();
            var image = UploadedImage(userModel.ProfilePicture);
            user.FirstName = userModel.FirstName;
            user.LastName = userModel.LastName;
            user.Contact = userModel.Contact;
            user.Cnic = userModel.Cnic;
            user.Email = userModel.Email;
            user.ImagePath = image;
            user.LastModify = DateTime.Now;
            user.IsActive = true;
            user.ResetCode = Convert.ToInt64("");
            
                var emailExist = await ExitingEmail(user.Email);
            var usernameExist = await ExistingUsername(user.FirstName, user.LastName);
            var contactExist = await ExistingContact(user.Contact);

            if (emailExist || usernameExist || contactExist)
            {
                return null;
            }
            else
            {
                _dbContext.Entry(user).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return user;
            }
        }


        //Delete User account
        public async Task<User> DeleteUser(Guid id)
        {
            var user = await _dbContext.Users.Where(x => x.Id == id && x.IsActive).AsNoTracking().FirstOrDefaultAsync();
            if (user != null)
            {
                _dbContext.Remove(user);
                await _dbContext.SaveChangesAsync();
                return user;
            }
            else
                return null;
        }

        //Archive user account
        public async Task<User> ArchiveUser(Guid id)
        {
            var user = await _dbContext.Users.Where(x => x.Id == id && x.IsActive).AsNoTracking().FirstOrDefaultAsync();
            if (user != null)
            {
                user.IsActive = false;
                await _dbContext.SaveChangesAsync();
                return user;
            }
            else return null;
        }


        //Validation For Username , Email Or Contat no

        public async Task<bool> ExistingContact(string contact)
        {
            bool contactExist = await _dbContext.Users.AnyAsync(x => x.Contact == contact && x.IsActive);
            if (contactExist)
                return true;
            else
                return false;
        }

        public async Task<bool> ExistingUsername(string firstName, string lastName)
        {
            bool usernameExist = await _dbContext.Users.AnyAsync(x => x.FirstName == firstName && x.LastName == lastName && x.IsActive);
            if (usernameExist)
                return true;
            else
                return false;
        }

        public async Task<bool> ExitingEmail(string email)
        {
            bool emailExist = await _dbContext.Users.AnyAsync(x => x.Email == email && x.IsActive);
            if (emailExist)
                return true;
            else
                return false;
        }

        //Forget Password
        public async Task<User> ForgetPassowrd(string email)
        {
            var user = await _dbContext.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            if (user != null)
            {
                //Send code for reset password
                Random generator = new Random();
                string resetCode = generator.Next(0, 1000000).ToString("D6");
                var resetPassword = new UtilityClass();
                resetPassword.SendCodeToResetPassword(user.Email, resetCode);
                user.ResetCode = Convert.ToInt64(resetCode);
                _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
                await _dbContext.SaveChangesAsync();
                //message = "Reset password code has been sent to your Email.";
                return user;
            }
            else
            {
                return null;
            }
        }

        //ResetCode Validation 
        public async Task<bool> IsCodeValid(ForgetPassword forgetPassword)
        {
            return await _dbContext.Users.AnyAsync(x => x.Email == forgetPassword.Email && x.ResetCode == forgetPassword.ResetCode);
        }

        public async Task<User> ResetPassword(ResetPassword resetPassword)
        {
            var user = await _dbContext.Users.Where(x => x.Id == resetPassword.Id && x.IsActive).FirstOrDefaultAsync();
            if (user != null)
            {
                user.Password = resetPassword.Password;
                user.LastModify = DateTime.Now;
                _dbContext.Entry(resetPassword).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return user;
            }
            else return null;
        }

        //Image upload
        private string UploadedImage(IFormFile profilePicture)
        {
            string filePath = null;

            if (profilePicture != null)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "ProfileImages");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + profilePicture.FileName;
                filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    profilePicture.CopyTo(fileStream);
                }
            }
            return filePath;
        }
    }
}

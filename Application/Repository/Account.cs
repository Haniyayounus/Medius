using Application.IRepository;
using Application.ViewModels;
using Core;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Application.Repository
{
    public class Account : IAccount
    {
        private readonly MediusContext _dbContext;
        private bool _disposed;
        private readonly IConfiguration _config;

        public Account(MediusContext dbContext, IConfiguration config)
        {
            this._dbContext = dbContext;
            this._config = config;
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

        public async Task<User> SignUp(User user)
        {
            CheckNullOrEmpty(user);
            if (await IsUsernameDuplicate(user.FirstName, user.LastName)) throw new Exception($"'{user.FirstName}'+ '{user.LastName}' already exists. Please choose a different name.");
            if (await IsContactDuplicate(user.Contact)) throw new Exception($"'{user.Contact}' already exists. Please choose a different contact no.");
            if (await IsEmailDuplicate(user.Email)) throw new Exception($"'{user.Email}' already exists. Please choose a different email.");
            if (await IsCnicDuplicate(user.Cnic)) throw new Exception($"'{user.Cnic}' already exists. Please choose a different cnic.");

            int generateOTP =await GenerateOTP();
            string encodedPassword = await EncodePasswordToBase64(user.Password);

            user.RoleId = 1;
            user.OTP = generateOTP;
            user.Password = encodedPassword;
            user.ImagePath = "Placeholder.png";
            user.CreatedAt = DateTime.Now;
            user.LastModify = DateTime.Now;
            user.IsActive = true;
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUserById(Guid id)
        {
            if (id == null) throw new Exception($"Id is required.");

            return await _dbContext.Users.Where(x => x.Id == id && x.IsActive).FirstOrDefaultAsync() ?? throw new Exception($"No user found against Id:'{id}'");
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _dbContext.Users.Where(x => x.IsActive).AsNoTracking().ToListAsync();
        }

        public async Task<int> GenerateOTP()
        {
            await Task.Delay(1000);
            Random generator = new Random();
            var Otp = generator.Next(0, 1000000).ToString("D6");
            int OTPCode = (Convert.ToInt32(Convert.ToInt64(Otp)));
            return OTPCode;
        }

        public async Task<string> EncodePasswordToBase64(string password)
        {
            byte[] encryptData = new byte[password.Length];
            encryptData = System.Text.Encoding.UTF8.GetBytes(password);
            string encodedData = Convert.ToBase64String(encryptData);
            await Task.Delay(10000);
               return encodedData;
        }
        
        public async Task<User> ValidateOTP(Guid id, int OTP)
        {
            return await _dbContext.Users.Where(x => x.Id == id && x.OTP == OTP && x.IsActive).AsNoTracking().FirstOrDefaultAsync() ?? throw new Exception($"Wrong OTP against Id :'{id}'");
        }
        
        public async Task<User> ValidateResetCode(Guid id, long resetCode)
        {
            return await _dbContext.Users.Where(x => x.Id == id && x.ResetCode == resetCode && x.IsActive).AsNoTracking().FirstOrDefaultAsync() ?? throw new Exception($"Wrong OTP against Id :'{id}'");
        }

        public void CheckNullOrEmpty(User user)
        {
            if (user.FirstName == null || user.FirstName == "") throw new Exception($"First Name is required. Please write a name.");
            if (user.LastName == null || user.LastName == "") throw new Exception($"Last Name is required. Please write a name.");
            if (user.Contact == null || user.Contact == "") throw new Exception($"Contact no is required. Please write a contact no.");
            if (user.Cnic == null || user.Cnic == "") throw new Exception($"Cnic is required. Please write a cnic.");
            if (user.Email == null || user.Email == "") throw new Exception($"Email no is required. Please write an email.");
            if (user.Password == null || user.Password == "") throw new Exception($"Password is required. Please write a password.");
        }

        public Task<bool> IsContactDuplicate(string contact)
            => _dbContext.Users.AnyAsync(x => x.Contact.Equals(contact) && x.IsActive);
        
        public Task<bool> IsEmailDuplicate(string email)
            => _dbContext.Users.AnyAsync(x => x.Email.ToLower().Equals(email.ToLower()) && x.IsActive);
        
        public Task<bool> IsUsernameDuplicate(string firstname, string lastname)
            => _dbContext.Users.AnyAsync(x => x.FirstName.ToLower().Equals(firstname.ToLower()) && x.LastName.ToLower().Equals(lastname.ToLower()) && x.IsActive);
        
        public Task<bool> IsCnicDuplicate(string cnic)
                   => _dbContext.Users.AnyAsync(x => x.Cnic.Equals(cnic) && x.IsActive);

        public async Task<User> Login(string email, string pass)
        {
            string password = await EncodePasswordToBase64(pass);
            return await _dbContext.Users.Where(x => x.Email == email && x.Password == pass && x.IsActive).SingleOrDefaultAsync();
        }


        //delete account
        public async Task<User> DeleteMyAccount(Guid id)
        {
            var user = await GetUserById(id);
            _dbContext.Remove(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> DeactivateMyAccount(Guid id)
        {
            var user = await GetUserById(id);
            user.IsActive = false;
            user.LastModify = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return user;
        }


        //Profile
        public async Task<User> GetProfile(Guid id)
        {
            return await _dbContext.Users.Where(x => x.Id == id && x.IsActive).FirstOrDefaultAsync();
        }

        public async Task<User> ModifyProfile(UserModel user)
        {
            var getUser = await GetUserById(user.Id);

            getUser.FirstName = user.FirstName;
            getUser.LastName = user.LastName;
            getUser.Email = user.Email;
            getUser.Contact = user.Contact;
            getUser.Cnic = user.Cnic;
            getUser.ImagePath = user.ImagePath;
            getUser.LastModify = DateTime.Now;

            _dbContext.Entry(getUser).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return getUser;
        }


        //Password
        public async Task<User> ChangePassword(Guid id, string pass, string newPass)
        {
            var user = await _dbContext.Users.Where(x => x.Id == id && x.Password == pass && x.IsActive).SingleOrDefaultAsync() ?? throw new Exception($"Incorrect password against Id:'{id}'");
            
            user.Password = newPass;
            user.LastModify = DateTime.Now;
            return user;
        }
        
        public async Task<User> ForgetPassword(string email)
        {
            var account = await _dbContext.Users.Where(x => x.Email == email).FirstOrDefaultAsync() ?? throw new Exception($"No user found against Email:'{email}'");
            if (account != null)
            {
                //Send code for reset password
                await GenerateOTP();
                Random generator = new Random();
                string resetCode = generator.Next(0, 1000000).ToString("D6");
                await SendCodeToResetPassword(account.Email, resetCode);
                account.ResetCode = Convert.ToInt64(resetCode);

                _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
                await _dbContext.SaveChangesAsync();
            }
            return account;
        }

        public async Task<string> SendCodeToResetPassword(string email, string resetCode)
        {
            var from = _config.GetValue<string>("EmailConfiguration:From");
            var host = _config.GetValue<string>("EmailConfiguration:SmtpServer");
            string port = _config.GetValue<string>("EmailConfiguration:Port");
            string password = _config.GetValue<string>("EmailConfiguration:Password");
            var fromEmail = new MailAddress(from);
            var toEmail = new MailAddress(email);
            var fromEmailPassword = password;
            var subject = "Reset Password";
            var body = "Hi,<br/>br/>We got request for reset your account password. Reset your password by entering this 6-digit code" +
                    "<br/><br/><strong>Reset Password code " + resetCode + "</strong>";
            var smtp = new SmtpClient
            {
                Host = host,
                Port = Convert.ToInt32(port),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
            await Task.Delay(10000);
            return "message sent";
        }

        public async Task<User> ResetPassword(Guid id, string newPass)
        {
            var user = await GetUserById(id);
            user.Password = newPass;
            user.LastModify = DateTime.Now;
            _dbContext.Entry(user).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return user;
        }

    }
}

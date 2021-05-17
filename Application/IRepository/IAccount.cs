using Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.IRepository
{
    public interface IAccount
    {
        Task<List<User>> GetAllUsers();
        Task<User> SignUp(User user);
        Task<User> Login(string email, string pass);
        Task<User> DeleteMyAccount(Guid id);
        Task<User> DeactivateMyAccount(Guid id);
        Task<User> GetProfile(Guid id);
        Task<User> ModifyProfile(User user);
        Task<User> GetUserById(Guid id);
        Task<User> ChangePassword(Guid id, string pas , string newPass);
        Task<User> ValidateOTP(Guid id, int OTP);
        Task<User> ValidateResetCode(Guid id, long resetCode);
        Task<User> ForgetPassword(string email);
        Task<User> ResetPassword(Guid id, string newPass);
        void CheckNullOrEmpty(User user);
        Task<bool> IsContactDuplicate(string contact);
        Task<bool> IsCnicDuplicate(string cnic);
        Task<bool> IsEmailDuplicate(string email);
        Task<bool> IsUsernameDuplicate(string firstname, string lastname);
        Task<string> SendCodeToResetPassword(string email, string resetCode);

        Task<int> GenerateOTP();
        Task<string> EncodePasswordToBase64(string password);
    }
}

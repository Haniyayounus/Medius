using Microsoft.AspNetCore.Http;
using System;

namespace Application.ViewModels
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Cnic { get; set; }
        public string Contact { get; set; }
        public IFormFile ProfilePicture { get; set; }
        public string ImagePath { get; set; }
        public string FileDocument { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }
    public class ForgetPassword
    {
        public string Email { get; set; }
        public long ResetCode { get; set; }
    }
    public class ResetPassword
    {
        public Guid Id { get; set; }
        public string Password { get; set; }

    }
}

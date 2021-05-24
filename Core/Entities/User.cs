using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class User : BaseEntity<Guid>
    {
        public override Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Cnic { get; set; }
        public string Contact { get; set; }
        [NotMapped]
        public IFormFile ProfilePicture { get; set; }
        public string ImagePath { get; set; }
        public int RoleId { get; set; }
        public long? ResetCode { get; set; }
        public int? OTP { get; set; }


        //Relationship
        public virtual UserRole UserRole { get; set; }
    }
}

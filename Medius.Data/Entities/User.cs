using Medius.Data.Enums;
using System;
using System.Collections.Generic;
namespace Medius.Data.Entities
{
    public class User : BaseEntity
    {
        public User()
        {
            AccountType = AccountType.User;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Cnic { get; set; }
        public string Contact { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string ResetCode { get; set; }
        public string Image { get; set; }
        public AccountType AccountType { get; set; }
    }
}

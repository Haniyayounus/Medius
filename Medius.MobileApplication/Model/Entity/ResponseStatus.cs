
using System;

namespace Medius.MobileApplication.Model.Entity
{
    public class ResponseStatus
    {
        public string Rcode { get; set; }
        public string Message { get; set; }
    }
    public class LoginEntity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Cnic { get; set; }
        public string Contact { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
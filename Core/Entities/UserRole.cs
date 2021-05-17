using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class UserRole : BaseEntity<int>
    {
        public override int Id { get; set; }
        public UserType Role { get; set; }
        public string Description { get; set; }
    }
}

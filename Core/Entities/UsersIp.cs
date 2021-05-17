using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class UsersIp : BaseEntity<Guid>
    {
        public override Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public IpType IpType { get; set; }
        public string Contact { get; set; }
        public string Application { get; set; }
        public IpStatus Status { get; set; }
        public string Image { get; set; }
        public string DocumentPath { get; set; }

        [NotMapped]
        public IFormFile FileDocument { get; set; }
        public Guid ClaimId { get; set; }
        public int CityId { get; set; }
        public Guid UserId { get; set; }
        public int? IpFilterId { get; set; }

        //Relationships
        public virtual Claim Claim { get; set; }
        public virtual City City { get; set; }
        public virtual User User { get; set; }
        public virtual IpFilter IpFilter { get; set; }
    }
}

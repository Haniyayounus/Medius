using System;

namespace Core.Entities
{
    public class Claim : BaseEntity<Guid>
    {
        public override Guid Id { get; set; } = Guid.NewGuid();
        public int ClaimNo { get; set; }
        public string Description { get; set; }
    }
}

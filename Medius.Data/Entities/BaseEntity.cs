using System;

namespace Medius.Data.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            Id = new Guid();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            IsDeleted = false;
        }
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}

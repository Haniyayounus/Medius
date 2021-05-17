using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class FAQ : BaseEntity<Guid>
    {
        public override Guid Id { get; set; } = Guid.NewGuid();
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}

using System;

namespace Core.Entities
{
    public class IpFilter : BaseEntity<int>
    {
        public override int Id { get; set; }
        public FilterType Type { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}

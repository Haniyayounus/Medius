using System;

namespace Core.Entities
{
    public class City : BaseEntity<int>
    {
        public override int Id { get; set; }
        public string CityName { get; set; }
    }
}

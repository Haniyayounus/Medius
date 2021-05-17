
using Medius.Data.Entities;
using System.Data.Entity;

namespace Medius.Data
{
    public class MediusContext : DbContext
    {
        public MediusContext()
        {

        }
        public DbSet<User> Users { get; set; }
    }
}

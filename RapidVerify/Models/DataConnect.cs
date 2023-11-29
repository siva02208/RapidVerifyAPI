using Microsoft.EntityFrameworkCore;

namespace PassportVerification.Models
{
    public class DataConnect:DbContext
    {
        public DataConnect(DbContextOptions<DataConnect> options) : base(options)
        {

        }
        public DbSet<VerifiedUser> VerifiedUsers { get; set; }
    }
}

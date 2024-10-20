using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWorks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "7c0f36aa-a714-40f8-8167-a777c7591cb7";
            var writeRoleId = "07b53ecd-9976-4685-b136-328891c700ad";

            var roles = new List<IdentityRole>
            {
                new()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper()
                },
                new()
                {
                    Id = writeRoleId,
                    Name = "Writer",
                    NormalizedName = "Write".ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using NZWorks.API.Models.Domain;

namespace NZWorks.API.Data
{
    public class NZWalksDBContext : DbContext
    {
        public NZWalksDBContext(DbContextOptions<NZWalksDBContext> dbContextOptions): base(dbContextOptions)
        {
            
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data for Difficulties
            // Easy, Medium, Hard

            var difficulties = new List<Difficulty>()
            {
                new()
                {
                    Id = Guid.Parse("388d17c6-eda2-4e8f-a3b8-3912289396db"),
                    Name = "Easy"
                },
                new()
                {
                    Id =  Guid.Parse("5699a4f3-1f0d-4146-b30f-77c21c338ee5"),
                    Name = "Medium"
                },
                new()
                {
                    Id =  Guid.Parse("551ea2fe-4297-4363-945f-2903e67593e6"),
                    Name = "Hard"
                }
            };

            // Seed difficulties tho the database
            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            // Seed data for Difficulties
            // Easy, Medium, Hard

            var regions = new List<Region>()
            {
                new()
                {
                    Id = Guid.Parse("1c2f9436-3011-43ea-82f5-9dd0b541aa18"),
                    Name = "Auckland",
                    Code = "AKL",
                    RegionImageUrl = "www.imgakl.com",
                },
                new()
                {
                    Id =  Guid.Parse("9e7b4d39-0659-4a16-861e-c0755e8ffbc0"),
                    Name = "Johannesburg",
                    Code = "JHB",
                    RegionImageUrl = "www.imgjhb.com",
                },
                new()
                {
                    Id =  Guid.Parse("a7515634-ace7-4c87-9d7f-962509d7661b"),
                    Name = "Cape Town",
                    Code = "CPT",
                    RegionImageUrl = "www.imgcpt.com",
                }
            };

            // Seed difficulties tho the database
            modelBuilder.Entity<Region>().HasData(regions);
        }
    }
}

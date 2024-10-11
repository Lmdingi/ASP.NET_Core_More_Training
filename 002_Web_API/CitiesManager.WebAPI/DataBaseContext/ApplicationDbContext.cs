using System;
using CitiesManager.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.WebAPI.DataBaseContext;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    { }

    public virtual DbSet<City> Cities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<City>().HasData(
            new City()
            {
                CityID = Guid.Parse("cb74a9d4-adc4-4305-9a33-ea37742be0fe"),
                CityName = "New York"
            }
        );

        modelBuilder.Entity<City>().HasData(
            new City()
            {
                CityID = Guid.Parse("bf8cef02-a02d-4705-84d5-16eec285c8a7"),
                CityName = "London"
            }
        );
    }
}

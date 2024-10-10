using System.Text.Json;
using Entities;
using Microsoft.EntityFrameworkCore;

public class PersonsDbContext : DbContext
{
    public DbSet<Country> Countries { get; set; }
    public DbSet<Person> Persons { get; set; }
    public PersonsDbContext()
    { }
    public PersonsDbContext(DbContextOptions<PersonsDbContext> options) : base(options)
    { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=PersonsDatabase;Trusted_Connection=True;TrustServerCertificate=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Country>().ToTable("Countries");
        modelBuilder.Entity<Person>().ToTable("Persons");

        // Seed Countries
        string countriesJson = File.ReadAllText("countries.json");
        List<Country> countries = JsonSerializer.Deserialize<List<Country>>(countriesJson);
        foreach (Country country in countries)
        {
            modelBuilder.Entity<Country>().HasData(country);
        }

        // Seed Persons
        string personsJson = File.ReadAllText("persons.json");
        List<Person> persons = JsonSerializer.Deserialize<List<Person>>(personsJson);
        foreach (Person person in persons)
        {
            modelBuilder.Entity<Person>().HasData(person);
        }

        //Fluent API
        modelBuilder.Entity<Person>().Property(temp => temp.TIN)
            // .HasColumnName("TaxIdentificationNumber")
            .HasColumnType("varchar(8)")
            .HasDefaultValue("ABC12345");

        // modelBuilder.Entity<Person>(entity =>
        // {
        //     entity.HasOne<Country>(c => c.Country)
        //     .WithMany(p => p.Persons)
        //     .HasForeignKey(p => p.CountryID);
        // });
    }
}

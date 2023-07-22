using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts;

public class CityInfoContext : DbContext
{
    public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
    {
    }

    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<PointOfInterest> PointOfInterests { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>().HasData(
            new City("New York City")
            {
                Id = 1,
                Description = "The one with that big park."
            },
            new City("Antwerp")
            {
                Id = 2,
                Description = "The one that is in Belgium."
            },
            new City("Tangier City")
            {
                Id = 3,
                Description = "The one with that big port between Morocco and Spain."
            }
        );

        modelBuilder.Entity<PointOfInterest>().HasData(
            new PointOfInterest("Central Park")
            {
                Id = 1,
                CityId = 1,
                Description = "The most visited central park in the world."
            },
            new PointOfInterest("Empire State Building")
            {
                Id = 2,
                CityId = 1,
                Description = "A 102-story skyscraper located in Midtown Manhattan."
            },
            new PointOfInterest("Big Port")
            {
                Id = 3,
                CityId = 3,
                Description = "Is the big port in Africa, and one of the biggest in the world."
            }
        );
        base.OnModelCreating(modelBuilder);
    }
}
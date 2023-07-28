using CityInfo.API.Models;

namespace CityInfo.API;

public class CitiesDataStore
{
    public CitiesDataStore()
    {
        Cities = new List<CityDto>
        {
            new()
            {
                Id = 1,
                Name = "New York",
                Description = "The one with that big park.",
                PointOfInterest = new List<PointOfInterestDto>()
                {
                    new()
                    {
                        Id = 1,
                        Name = "Central Park",
                        Description = "The most visited park in the world."
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Brooklyn",
                        Description = "The most visited park in the world."
                    },
                }
            },
            new()
            {
                Id = 2,
                Name = "Antwerp",
                Description = "The one with the cathedral that was never finished.",
                PointOfInterest = new List<PointOfInterestDto>()
                {
                    new()
                    {
                        Id = 1,
                        Name = "Cathedral",
                        Description = "The most visited cathedral in the world."
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Nations Park",
                        Description = "The most visited park in the world."
                    },
                }
            },
            new()
            {
                Id = 3,
                Name = "Tangier",
                Description = "The one with vast valley in Africa.",
                PointOfInterest = new List<PointOfInterestDto>()
                {
                    new()
                    {
                        Id = 1,
                        Name = "Nations Park",
                        Description = "The most visited park in the world."
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Coffe Over the Sea",
                        Description = "Al Haffa Coffe is the best coffe in Tangier."
                    },
                }
            }
        };
    }

    public List<CityDto> Cities { get; set; }
}
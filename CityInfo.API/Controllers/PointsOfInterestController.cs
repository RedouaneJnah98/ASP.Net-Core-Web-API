using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[Route("/api/cities/{cityId}/pointsofinterest")]
[ApiController]
public class PointsOfInterestController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

        if (city == null) return NotFound();

        return Ok(city.PointOfInterestDto);
    }

    [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
    public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

        if (city == null) return NotFound();

        // find point of interest
        var pointOfInterest = city.PointOfInterestDto.FirstOrDefault(p => p.Id == pointOfInterestId);

        if (pointOfInterest == null) return NotFound();

        return Ok(pointOfInterest);
    }

    [HttpPost]
    public ActionResult<PointOfInterestDto> CreatePointOfInterest(
        int cityId, PointOfInterestForCreationDto pointofinterest
    )
    {
        var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

        if (city == null) return NotFound();

        // demo purposes - to be improved
        var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(
            c => c.PointOfInterestDto).Max(p => p.Id);

        var finalPointOfInterest = new PointOfInterestDto
        {
            Id = ++maxPointOfInterestId,
            Name = pointofinterest.Name,
            Description = pointofinterest.Description
        };

        city.PointOfInterestDto.Add(finalPointOfInterest);

        return CreatedAtRoute("GetPointOfInterest",
            new
            {
                cityId,
                pointOfInterestId = finalPointOfInterest.Id
                // pointofinterest = finalPointOfInterest.Id
            },
            finalPointOfInterest);
    }
}
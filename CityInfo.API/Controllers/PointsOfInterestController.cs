using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[Route("/api/cities/{cityId}/pointsofinterest")]
[ApiController]
public class PointsOfInterestController : ControllerBase
{
    private readonly ILogger<PointsOfInterestController> _logger;
    private readonly IMailService _mailService;
    private readonly CitiesDataStore _citiesDataStore;

    public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService,
        CitiesDataStore citiesDataStore)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
        _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
    }

    [HttpGet]
    public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
    {
        try
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                _logger.LogInformation($"City with id {cityId} was not found when accessing points of interest.");
                return NotFound();
            }

            return Ok(city.PointOfInterestDto);
        }
        catch (Exception e)
        {
            _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}", e);

            return StatusCode(500, "A problem happened while handling your request.");
        }
    }

    [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
    public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
    {
        var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

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
        var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

        if (city == null) return NotFound();

        // demo purposes - to be improved
        var maxPointOfInterestId = _citiesDataStore.Cities.SelectMany(
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
            },
            finalPointOfInterest);
    }

    [HttpPut("{pointOfInterestId}")]
    public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId,
        PointOfInterestDto pointOfInterest)
    {
        var city = _citiesDataStore.Cities
            .FirstOrDefault(c => c.Id == cityId);
        if (city == null) return NotFound();

        // find point of interest
        var pointOfInterestFromStore = city.PointOfInterestDto
            .FirstOrDefault(c => c.Id == pointOfInterestId);

        if (pointOfInterestFromStore == null) return NotFound();

        pointOfInterestFromStore.Name = pointOfInterest.Name;
        pointOfInterestFromStore.Description = pointOfInterest.Description;

        return NoContent();
    }

    [HttpPatch("{pointOfInterestId}")]
    public ActionResult PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId,
        JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
    {
        var city = _citiesDataStore.Cities
            .FirstOrDefault(c => c.Id == cityId);
        if (city == null) return NotFound();

        var pointOfInterestFromStore = city.PointOfInterestDto
            .FirstOrDefault(c => c.Id == pointOfInterestId);

        if (pointOfInterestFromStore == null) return NotFound();

        var pointOfInterestToPatch = new PointOfInterestForUpdateDto
        {
            Name = pointOfInterestFromStore.Name,
            Description = pointOfInterestFromStore.Description
        };

        patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (!TryValidateModel(pointOfInterestToPatch)) return BadRequest(ModelState);

        pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
        pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

        return NoContent();
    }

    [HttpDelete("{pointOfInterestId}")]
    public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
    {
        var city = _citiesDataStore.Cities
            .FirstOrDefault(c => c.Id == cityId);
        if (city == null) return NotFound();

        var pointOfInterestFromStore = city.PointOfInterestDto
            .FirstOrDefault(c => c.Id == pointOfInterestId);

        if (pointOfInterestFromStore == null) return NotFound();

        city.PointOfInterestDto.Remove(pointOfInterestFromStore);

        _mailService.Send("Point of interest deleted.",
            $"Point of interest {pointOfInterestFromStore.Name} with id {pointOfInterestId} was deleted.");

        return NoContent();
    }
}
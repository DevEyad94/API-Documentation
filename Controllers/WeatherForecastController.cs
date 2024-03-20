using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NJsonSchema.NewtonsoftJson.Converters;
using NSwag.Annotations;

namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing",
        "Bracing",
        "Chilly",
        "Cool",
        "Mild",
        "Warm",
        "Balmy",
        "Hot",
        "Sweltering",
        "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [SwaggerResponse(
        StatusCodes.Status200OK,
        typeof(WeatherForecast),
        Description = "Product found and returned successfully."
    )]
    [SwaggerResponse(
        StatusCodes.Status500InternalServerError,
        typeof(string),
        Description = "Error in Server"
    )]
    [SwaggerResponse(
        StatusCodes.Status404NotFound,
        typeof(string),
        Description = "Product not found."
    )]
    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        try
        {
            return Enumerable
                .Range(1, 5)
                .Select(
                    index =>
                        new WeatherForecast
                        {
                            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                            TemperatureC = Random.Shared.Next(-20, 55),
                            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                        }
                )
                .ToArray();
        }
        catch (LocationNotFoundException locationNotFoundException)
        {
            return (IEnumerable<WeatherForecast>)StatusCode(500, locationNotFoundException);
        }
    }

    [JsonConverter(typeof(JsonExceptionConverter))]
    public class LocationNotFoundException : Exception
    {
        [JsonProperty("location")]
        public string Location { get; }

        public LocationNotFoundException(string location, Exception exception)
            : base("The location could not be found.", exception)
        {
            Location = location;
        }
    }

    // [HttpGet("getData")]
    // public IEnumerable<String> getData(){

    // }
}

namespace SubscriptionManagement.API.Controllers;

public class WeatherForecastController : BaseController
{
	private readonly ILogger<WeatherForecastController> _logger;

	public WeatherForecastController(ILogger<WeatherForecastController> logger)
	{
		_logger = logger;
	}
}
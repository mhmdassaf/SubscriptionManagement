namespace SubscriptionManagement.API.Controllers;

[Authorize]
public class SubscriptionController : BaseController
{
	private readonly ISubscriptionService _subscriptionService;

	public SubscriptionController(ISubscriptionService subscriptionService)
	{
		_subscriptionService = subscriptionService;
	}

	/// <summary>
	/// Get the Subscription based on userId you are passed
	/// </summary>
	/// <param name="userId"> the userId that related to the Subscription</param>
	/// <returns>list of Subscription</returns>

	[HttpGet]
	public async Task<IActionResult> GetByUserId([Required] string userId)
	{
		var response = await _subscriptionService.GetByUserId(userId);
		return StatusCode((int)response.Code, response);
	}

	/// <summary>
	/// Get all active subscription, 
	/// if no subscription are found retry 3 times before return the response
	/// </summary>
	/// <returns>list of Subscription</returns>
	[HttpGet]
	public async Task<IActionResult> GetActive()
	{
		var response = await _subscriptionService.GetActive();
		return StatusCode((int)response.Code, response);
	}

	/// <summary>
	/// Calculate the Remaining Days of a Subscription by passing the sub id
	/// </summary>
	/// <param name="dto">SubscriptionId: valid sub id</param>
	/// <returns>the remaining days of a Subscription</returns>
	[HttpPost]
	public async Task<IActionResult> CalculateRemainingDays(CalculateRemainingDaysDto dto)
	{
		var response = await _subscriptionService.CalculateRemainingDays(dto);
		return StatusCode((int)response.Code, response);
	}
}

namespace SubscriptionManagement.Common.Models.Subscription;

public class SubscriptionModel
{
	public string UserId { get; set; } = null!;
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public string SubscriptionType { get; set; } = null!;
}

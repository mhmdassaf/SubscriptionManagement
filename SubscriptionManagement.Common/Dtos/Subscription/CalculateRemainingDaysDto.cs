namespace SubscriptionManagement.Common.Dtos.Subscription;

public class CalculateRemainingDaysDto
{
    [Required]
    public Guid SubscriptionId { get; set; }
}

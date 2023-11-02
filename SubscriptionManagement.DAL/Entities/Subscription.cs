namespace SubscriptionManagement.DAL.Entities;

public class Subscription : BaseEntity<Guid>
{
	public string UserId { get; set; } = null!;

	[ForeignKey(nameof(UserId))]
	public User User { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

	[Required]
	public string SubscriptionType { get; set; } = null!;

	[Required]
    public double Price { get; set; }
}

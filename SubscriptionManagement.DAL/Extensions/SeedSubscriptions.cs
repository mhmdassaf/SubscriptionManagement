namespace SubscriptionManagement.DAL.Entities;

public static partial class Seed
{
	public static void SeedSubscriptions(this ModelBuilder builder)
	{
		builder.Entity<Subscription>().HasData(new List<Subscription>
		{
		});
	}
}

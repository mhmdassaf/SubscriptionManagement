namespace SubscriptionManagement.DAL.Entities;

public static partial class Seed
{
	public static void SeedUsers(this ModelBuilder builder)
	{
		builder.Entity<User>().HasData(new List<User>
		{
		});
	}
}

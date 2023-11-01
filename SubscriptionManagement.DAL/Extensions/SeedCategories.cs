namespace SubscriptionManagement.DAL.Entities;

public static partial class Seed
{
	public static void SeedCategories(this ModelBuilder builder)
	{
		builder.Entity<Category>().Property(x => x.Id).ValueGeneratedNever();

		builder.Entity<Category>().HasData(new List<Category>
		{
			new Category
			{
				Id = 1
			}
		});
	}
}

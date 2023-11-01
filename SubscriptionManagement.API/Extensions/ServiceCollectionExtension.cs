using ManageProducts.BAL.Services;

namespace SubscriptionManagement.API.Extensions;

public static class ServiceCollectionExtension
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		services.AddScoped<ICategoryService, CategoryService>();
		return services;
	}

	public static IServiceCollection AddAuth(this IServiceCollection services)
	{
		services.AddAuthentication();
		return services;
	}

	public static IServiceCollection AddDatabase<TContext>(this IServiceCollection services, ConfigurationManager configuration)
		where TContext : DbContext
	{
		services.AddDbContext<TContext>(config =>
		{
			config.UseNpgsql(configuration.GetConnectionString(nameof(ConnectionStrings.SubscriptionManagementConnection)));
		});

		return services;
	}

	public static IServiceCollection AddMapper(this IServiceCollection services)
	{
		services.AddAutoMapper(config =>
		{
			config.AddProfile<MappingProfile>();
		});
		return services;
	}
}

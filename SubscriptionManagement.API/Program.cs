var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Info("init main");

try
{
	var builder = WebApplication.CreateBuilder(args);

	#region Add services to the container.
	builder.Services.AddControllers();
	builder.Services.AddHttpContextAccessor();
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddApplicationServices(builder.Configuration);
	builder.Services.AddSwagger();
	builder.Services.AddAuth();
	builder.Services.AddDatabase<SubscriptionManagementDbContext>(builder.Configuration);
	builder.Services.AddGenericRepository<SubscriptionManagementDbContext>();
	builder.Services.AddMapper();
	#endregion

	var app = builder.UseNLog().Build();

	#region Configure the HTTP request pipeline.
	app.UseSwaggerInternal();
	app.UseHttpsRedirection();
	app.UseAuthentication();
	app.UseAuthorization();
	app.MapControllers();
	app.UseGlobalExceptionHandler();
	//app.ApplyMigration<SubscriptionManagementDbContext>();
	#endregion

	app.Run();
}
catch (Exception exception)
{
	// NLog: catch setup errors
	logger.Error(exception, "Stopped program because of exception");
	throw;
}
finally
{
	// Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
	LogManager.Shutdown();
}


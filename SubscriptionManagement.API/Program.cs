var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Info("init main");

try
{
	var builder = WebApplication.CreateBuilder(args);

	#region Add services to the container.
	builder.Services.AddControllers();
	builder.Services.AddApplicationServices(builder.Configuration);
	builder.Services.AddDatabase<SubscriptionManagementDbContext>(builder.Configuration);
	builder.Services.AddGenericRepository<SubscriptionManagementDbContext>();
	builder.Services.AddIdentityInternal();
	builder.Services.AddAuth();
	builder.Services.AddHttpContextAccessor();
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwagger();
	builder.Services.AddMapper();
	#endregion

  var app = builder.Build();

	#region Configure the HTTP request pipeline.
	app.UseSwaggerInternal();
	app.UseHttpsRedirection();
	app.UseAuthentication();
	app.UseAuthorization();
	app.MapControllers();
	app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
	app.UseGlobalExceptionHandler();
	app.ApplyMigration<SubscriptionManagementDbContext>();
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


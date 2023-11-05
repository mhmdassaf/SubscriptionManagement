namespace SubscriptionManagement.API.Extensions;

public static class ApplicationBuilderExtension
{
	public static WebApplication UseSwaggerInternal(this WebApplication app)
	{
		if (app.Environment.IsDevelopment())
		{
			var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

			app.UseSwagger();
			app.UseSwaggerUI(options =>
			{
				foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
				{
					options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
						description.GroupName.ToUpperInvariant());
				}
			});
		}
		return app;
	}

	public static WebApplication ApplyMigration<TDbContext>(this WebApplication app) where TDbContext : DbContext
	{
		var serviceScope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope();
		if (serviceScope == null) return app;

		var dbContext = serviceScope.ServiceProvider.GetRequiredService<TDbContext>();
		dbContext.Database.Migrate();
		return app;
	}

	public static WebApplicationBuilder UseNLog(this WebApplicationBuilder builder)
	{
		builder.Logging.ClearProviders();
		builder.Host.UseNLog();
		return builder;
	}

	public static WebApplication UseGlobalExceptionHandler(this WebApplication app)
	{
		app.UseExceptionHandler(errorApp =>
		{
			errorApp.Run(async context =>
			{
				var responseModel = new ResponseModel();
				var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

				var httpStatusCode = HttpStatusCode.OK;
				switch (exception)
				{
					case UnauthorizedAccessException ex:
						httpStatusCode = HttpStatusCode.Unauthorized;
						responseModel.Errors.Add(new ErrorModel
						{
							Code = HttpStatusCode.Unauthorized.ToString(),
							Message = ex.Message
						});
						break;
					default:
						httpStatusCode = HttpStatusCode.InternalServerError;
						responseModel.Errors.Add(new ErrorModel
						{
							Code = HttpStatusCode.InternalServerError.ToString(),
							Message = exception?.Message ?? "Internal server error!"
						});
						break;
				}

				context.Response.ContentType = "application/json";
				context.Response.StatusCode = (int)httpStatusCode;
				await context.Response.WriteAsync(JsonConvert.SerializeObject(responseModel));
			});
		});
		return app;
	}
}

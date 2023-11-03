namespace SubscriptionManagement.API.Extensions;

public static class ServiceCollectionExtension
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services, ConfigurationManager configuration)
	{
		services.Configure<AuthSettings>(configuration.GetSection(nameof(AuthSettings)));
		services.AddScoped<IAuthService, AuthService>();
		services.AddScoped<ISubscriptionService, SubscriptionService>();
		return services;
	}

	public static IServiceCollection AddSwagger(this IServiceCollection services)
	{
		services.AddSwaggerGen(options => {
			options.SwaggerDoc("V1", new OpenApiInfo
			{
				Version = "V1",
				Title = "SubscriptionManagement API"
			});
			options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				Scheme = "Bearer",
				BearerFormat = "JWT",
				In = ParameterLocation.Header,
				Name = "Authorization",
				Description = "Bearer Authentication with JWT Token",
				Type = SecuritySchemeType.Http
			});
			options.AddSecurityRequirement(new OpenApiSecurityRequirement {
			{
				new OpenApiSecurityScheme {
					Reference = new OpenApiReference {
						Id = "Bearer",
						Type = ReferenceType.SecurityScheme
					}
				},
				new List < string > ()
		    }});
		});
		return services;
	}

	public static IServiceCollection AddAuth(this IServiceCollection services)
	{
		var authSettings = services.BuildServiceProvider().GetService<IOptions<AuthSettings>>();
		if (authSettings == null || authSettings.Value == null) 
		{ 
			throw new ArgumentNullException(nameof(authSettings)); 
		}

		services.AddAuthentication(opt => {
			opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(options => {
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = authSettings.Value.ValidateIssuer,
				ValidateAudience = authSettings.Value.ValidateAudience,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = authSettings.Value.Issuer,
				ValidAudience = authSettings.Value.Audience,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Value.SecretKey))
			};
		});
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

	public static IServiceCollection AddIdentityInternal(this IServiceCollection services)
	{
		services.AddIdentity<User, IdentityRole>(config =>
		{
			config.Password.RequiredLength = 4;
			config.Password.RequireNonAlphanumeric = false;
			config.Password.RequireUppercase = false;
			config.Password.RequireDigit = false;
			config.SignIn.RequireConfirmedEmail = true;
		})
				.AddEntityFrameworkStores<SubscriptionManagementDbContext>()
				.AddDefaultTokenProviders();

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

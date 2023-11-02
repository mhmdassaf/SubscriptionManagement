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
		services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc("v1", new OpenApiInfo { Title = "SubscriptionManagement API", Version = "v1" });

			// Define the security requirements
			c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				In = ParameterLocation.Header,
				Description = "Please enter token",
				Name = "Authorization",
				Type = SecuritySchemeType.Http,
				BearerFormat = "JWT",
				Scheme = "bearer"
			});
			c.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					new string[] { }
				}
			});
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

		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(options =>
		{
			options.RequireHttpsMetadata = authSettings.Value.RequireHttpsMetadata; 
			options.SaveToken = true;
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authSettings.Value.SecretKey)),
				ValidateIssuer = authSettings.Value.ValidateIssuer,
				ValidateAudience = authSettings.Value.ValidateAudience,
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

	public static IServiceCollection AddIdentity(this IServiceCollection services)
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

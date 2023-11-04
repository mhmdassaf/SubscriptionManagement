namespace SubscriptionManagement.DAL.DataContext;

public class SubscriptionManagementDbContext : IdentityDbContext<User>
{
	private readonly HttpContext? _httpContext;

	public SubscriptionManagementDbContext(DbContextOptions options, IHttpContextAccessor? httpContextAccessor) : base(options)
	{
		_httpContext = httpContextAccessor?.HttpContext;
	}

	public DbSet<Subscription> Subscriptions { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.SeedSubscriptions();
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		string? userId = _httpContext?.User.FindFirstValue("sub");

		foreach (var entity in ChangeTracker.Entries())
		{
			var createDatePropName = nameof(BaseEntity<dynamic>.CreateDate);
			var createdByUserIdPropName = nameof(BaseEntity<dynamic>.CreatedByUserId);

			//Add Entity
			if (entity.State == EntityState.Added &&
				entity.Entity.GetType().GetProperty(createDatePropName) != null &&
				entity.Entity.GetType().GetProperty(createdByUserIdPropName) != null)
			{
				entity.Property(createDatePropName).CurrentValue = DateTime.UtcNow;
				entity.Property(createdByUserIdPropName).CurrentValue = userId;
			}
			else if (entity.State == EntityState.Modified)
			{
				var updateDatePropName = nameof(BaseEntity<dynamic>.UpdateDate);
				var updatedByUserIdPropName = nameof(BaseEntity<dynamic>.UpdatedByUserId);
				var deleteDatePropName = nameof(BaseEntity<dynamic>.DeletedDate);
				var deletedByUserIdPropName = nameof(BaseEntity<dynamic>.DeletedByUserId);
				var isDeletedPropName = nameof(BaseEntity<dynamic>.IsDeleted);

				//Delete Entity
				if (entity.Entity.GetType().GetProperty(deleteDatePropName) != null &&
					entity.Entity.GetType().GetProperty(deletedByUserIdPropName) != null &&
					entity.Entity.GetType().GetProperty(isDeletedPropName) != null &&
					bool.TryParse(entity.Property(isDeletedPropName).CurrentValue?.ToString(), out bool isDeleted) &&
					isDeleted)
				{
					entity.Property(deleteDatePropName).CurrentValue = DateTime.UtcNow;
					entity.Property(deletedByUserIdPropName).CurrentValue = userId;
				}

				//Update Entity
				else if (entity.Entity.GetType().GetProperty(updateDatePropName) != null &&
						 entity.Entity.GetType().GetProperty(updatedByUserIdPropName) != null)
				{
					entity.Property(updateDatePropName).CurrentValue = DateTime.UtcNow;
					entity.Property(updatedByUserIdPropName).CurrentValue = userId;
				}
			}
		}

		return base.SaveChangesAsync(cancellationToken);
	}
}

namespace ManageProducts.BAL.Services;

public class CategoryService : BaseService, ICategoryService
{
	public CategoryService(IRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(repository, mapper, httpContextAccessor)
	{
	}
}

public interface ICategoryService
{
}
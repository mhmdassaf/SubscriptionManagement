namespace SubscriptionManagement.BAL.Services.Base;

public abstract class BaseService
{
	protected readonly IRepository _repository;

	protected readonly IMapper _mapper;

	protected readonly HttpContext? _httpContext;

	public string UserId
	{
		get
		{
			string? text = _httpContext?.User.FindFirstValue("sub");
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new UnauthorizedAccessException("UserId is null");
			}

			return text;
		}
	}

	public ResponseModel ResponseModel { get; } = new ResponseModel();

	public BaseService(IRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
	{
		_repository = repository;
		_mapper = mapper;
		_httpContext = httpContextAccessor.HttpContext;
	}
}

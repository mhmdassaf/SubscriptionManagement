﻿namespace SubscriptionManagement.BAL.Services.Base;

public abstract class BaseService
{
	protected readonly IRepository _repository;
	protected readonly IMapper _mapper;
	protected readonly ILogger<BaseService> _logger;
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

	public BaseService(IRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
		ILogger<BaseService> logger)
	{
		_repository = repository;
		_mapper = mapper;
		_logger = logger;
		_httpContext = httpContextAccessor.HttpContext;
	}
}

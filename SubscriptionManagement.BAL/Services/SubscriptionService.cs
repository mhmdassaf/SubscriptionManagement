using System.Net;

namespace SubscriptionManagement.BAL.Services;

public class SubscriptionService : BaseService, ISubscriptionService
{
	public SubscriptionService(IRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
		ILogger<SubscriptionService> logger) : base(repository, mapper, httpContextAccessor, logger)
	{
	}

	public async Task<ResponseModel> CalculateRemainingDays(CalculateRemainingDaysDto dto)
	{
		var spec = new Specification<Subscription>
		{
			Conditions = new List<Expression<Func<Subscription, bool>>>
			{
				sub => sub.Id == dto.SubscriptionId
			}
		};

		var remainingDays = await _repository.GetAsync(spec, s => s.EndDate.Subtract(s.StartDate).Days);

		ResponseModel.Code = HttpStatusCode.OK;
		ResponseModel.Message = Validation.SuccessMsg;
		ResponseModel.Result = remainingDays;
		_logger.LogInformation($"{nameof(CalculateRemainingDays)} {Validation.SuccessMsg}");
		return ResponseModel;
	}

	public async Task<ResponseModel> GetActive()
	{
		// if the response contain errors >> retry 3 times before return the result
		var retryPolicy = Policy
		  .Handle<Exception>()
		  .OrResult<ResponseModel>(r => r.Errors.Any()) 
		  .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

		int i = 0;
		var responseModel = await retryPolicy.ExecuteAsync(async () =>
		{
			var response = new ResponseModel();
			// kindly find the attached 'get_active_subscriptions' function on solution under 'PostgresSQL_Functions' folder
			var subscriptions = await _repository.GetFromRawSqlAsync<Subscription>("SELECT * FROM get_active_subscriptions()");

			if (subscriptions == null || !subscriptions.Any())
			{
				response.Code = HttpStatusCode.BadRequest;
				response.Errors.Add(new ErrorModel
				{
					Code = Validation.Subscription.NoActiveSubscriptionFoundCode,
					Message = Validation.Subscription.NoActiveSubscriptionFoundMsg
				});
				_logger.LogWarning($"{nameof(GetActive)} Retry {i++}");
				return response;
			}

			response.Code = HttpStatusCode.OK;
			response.Message = Validation.SuccessMsg;
			response.Result = _mapper.Map<List<SubscriptionModel>>(subscriptions);
			_logger.LogInformation($"{nameof(GetActive)} {Validation.SuccessMsg}");
			return response;
		});

		return responseModel;
	}

	public async Task<ResponseModel> GetByUserId(string userId)
	{
		var spec = new Specification<Subscription>
		{
			Conditions = new List<Expression<Func<Subscription, bool>>>
			{
				sub => sub.UserId == userId
			}
		};

		var subscriptions = await _repository.GetListAsync(spec);

		if (subscriptions == null || !subscriptions.Any())
		{
			ResponseModel.Code = HttpStatusCode.BadRequest;
			ResponseModel.Errors.Add(new ErrorModel
			{
				Code = Validation.Subscription.NoSubscriptionFoundCode,
				Message = Validation.Subscription.NoSubscriptionFoundMsg
			});
			_logger.LogWarning($"{nameof(GetByUserId)} {Validation.Subscription.NoSubscriptionFoundCode}");
			return ResponseModel;
		}

		ResponseModel.Code = HttpStatusCode.OK;
		ResponseModel.Message = Validation.SuccessMsg;
		ResponseModel.Result = _mapper.Map<List<SubscriptionModel>>(subscriptions);
		_logger.LogInformation($"{nameof(GetByUserId)} {Validation.SuccessMsg}");
		return ResponseModel;
	}
}

public interface ISubscriptionService
{
	Task<ResponseModel> CalculateRemainingDays(CalculateRemainingDaysDto dto);
	Task<ResponseModel> GetActive();
	Task<ResponseModel> GetByUserId(string userId);
}
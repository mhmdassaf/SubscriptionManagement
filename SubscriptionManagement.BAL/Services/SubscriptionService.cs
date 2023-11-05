using System.Net;

namespace SubscriptionManagement.BAL.Services;

public class SubscriptionService : BaseService, ISubscriptionService
{
	private readonly AsyncRetryPolicy<ResponseModel> _retryPolicy;

	public SubscriptionService(IRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
		ILogger<SubscriptionService> logger,
		RetryPolicyProvider retryPolicyProvider) : base(repository, mapper, httpContextAccessor, logger)
	{
		_retryPolicy = retryPolicyProvider.CreateRetryPolicy();
	}

	public async Task<ResponseModel> CalculateRemainingDays(CalculateRemainingDaysDto dto)
	{
		var responseModel = await _retryPolicy.ExecuteAsync(async () =>
		{
			var response = new ResponseModel();

			#region method-1
			//var spec = new Specification<Subscription>
			//{
			//	Conditions = new List<Expression<Func<Subscription, bool>>>
			//	{
			//		sub => sub.Id == dto.SubscriptionId
			//	}
			//};
			//var remainingDays = await _repository.GetAsync(spec, s => s.EndDate.Subtract(s.StartDate).Days); 
			#endregion

			#region method-2 >> using sql functions
			var result = await _repository.GetFromRawSqlAsync<int>($"SELECT calculate_remaining_days(@p0)", dto.SubscriptionId);
			var remainingDays = result.FirstOrDefault();
			#endregion

			response.Code = HttpStatusCode.OK;
			response.Message = Validation.SuccessMsg;
			response.Result = remainingDays;
			_logger.LogInformation($"{nameof(CalculateRemainingDays)} {Validation.SuccessMsg}");
			return response;
		});
		return responseModel;
	}

	public async Task<ResponseModel> GetActives()
	{
		var responseModel = await _retryPolicy.ExecuteAsync(async () =>
		{
			var response = new ResponseModel();
			var subscriptions = await _repository.GetFromRawSqlAsync<Subscription>("SELECT * FROM get_active_subscriptions()");

			if (subscriptions == null || !subscriptions.Any())
			{
				response.Code = HttpStatusCode.BadRequest;
				response.Errors.Add(new ErrorModel
				{
					Code = Validation.Subscription.NoActiveSubscriptionFoundCode,
					Message = Validation.Subscription.NoActiveSubscriptionFoundMsg
				});
				_logger.LogWarning($"{nameof(GetActives)} {Validation.Subscription.NoActiveSubscriptionFoundCode}");
				return response;
			}

			response.Code = HttpStatusCode.OK;
			response.Message = Validation.SuccessMsg;
			response.Result = _mapper.Map<List<SubscriptionModel>>(subscriptions);
			_logger.LogInformation($"{nameof(GetActives)} {Validation.SuccessMsg}");
			return response;
		});
		return responseModel;
	}

	public async Task<ResponseModel> GetByUserId(string userId)
	{
		var responseModel = await _retryPolicy.ExecuteAsync(async () =>
		{
			var response = new ResponseModel();

			#region Method-1
			var spec = new Specification<Subscription>
			{
				Conditions = new List<Expression<Func<Subscription, bool>>>
			{
				sub => sub.UserId == userId
			}
			};
			var subscriptions = await _repository.GetListAsync(spec);
			#endregion

			#region Method-2 >> using sql functions
			//var subscriptions = await _repository.GetFromRawSqlAsync<Subscription>($"SELECT * FROM get_subscriptions_by_user(@p0)", userId);
			#endregion

			if (subscriptions == null || !subscriptions.Any())
			{
				response.Code = HttpStatusCode.BadRequest;
				response.Errors.Add(new ErrorModel
				{
					Code = Validation.Subscription.NoSubscriptionFoundCode,
					Message = Validation.Subscription.NoSubscriptionFoundMsg
				});
				_logger.LogWarning($"{nameof(GetByUserId)} {Validation.Subscription.NoSubscriptionFoundCode}");
				return response;
			}

			response.Code = HttpStatusCode.OK;
			response.Message = Validation.SuccessMsg;
			response.Result = _mapper.Map<List<SubscriptionModel>>(subscriptions);
			_logger.LogInformation($"{nameof(GetByUserId)} {Validation.SuccessMsg}");
			return response;
		});
		return responseModel;
	}
}

public interface ISubscriptionService
{
	Task<ResponseModel> CalculateRemainingDays(CalculateRemainingDaysDto dto);
	Task<ResponseModel> GetActives();
	Task<ResponseModel> GetByUserId(string userId);
}
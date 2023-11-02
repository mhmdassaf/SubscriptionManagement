using System.Net;

namespace ManageProducts.BAL.Services;

public class SubscriptionService : BaseService, ISubscriptionService
{
	public SubscriptionService(IRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(repository, mapper, httpContextAccessor)
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
		return ResponseModel;
	}

	public async Task<ResponseModel> GetActive()
	{
		// kindly find the attached 'get_active_subscriptions' function on solution under 'PostgresSQL_Functions' folder
		var subscriptions = await _repository.GetFromRawSqlAsync<Subscription>("SELECT * FROM get_active_subscriptions()");
		
		if(subscriptions == null || !subscriptions.Any())
		{
			ResponseModel.Code = HttpStatusCode.NotFound;
			ResponseModel.Errors.Add(new ErrorModel
			{
				Code = Validation.Subscription.NoActiveSubscriptionFoundCode,
				Message = Validation.Subscription.NoActiveSubscriptionFoundMsg
			});
			return ResponseModel;
		}

		ResponseModel.Code = HttpStatusCode.OK;
		ResponseModel.Message = Validation.SuccessMsg;
		ResponseModel.Result = _mapper.Map<List<SubscriptionModel>>(subscriptions);
		return ResponseModel;
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
			ResponseModel.Code = HttpStatusCode.NotFound;
			ResponseModel.Errors.Add(new ErrorModel
			{
				Code = Validation.Subscription.NoSubscriptionFoundCode,
				Message = Validation.Subscription.NoSubscriptionFoundMsg
			});
			return ResponseModel;
		}

		ResponseModel.Code = HttpStatusCode.OK;
		ResponseModel.Message = Validation.SuccessMsg;
		ResponseModel.Result = _mapper.Map<List<SubscriptionModel>>(subscriptions);
		return ResponseModel;
	}
}

public interface ISubscriptionService
{
	Task<ResponseModel> CalculateRemainingDays(CalculateRemainingDaysDto dto);
	Task<ResponseModel> GetActive();
	Task<ResponseModel> GetByUserId(string userId);
}
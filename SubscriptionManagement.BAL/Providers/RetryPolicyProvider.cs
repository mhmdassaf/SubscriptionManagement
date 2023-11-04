namespace SubscriptionManagement.BAL.Providers;

public class RetryPolicyProvider
{
	public AsyncRetryPolicy<ResponseModel> CreateRetryPolicy()
	{
		// if the response contain errors >> retry 2 times before return the result
		var retryPolicy = Policy
		  .Handle<Exception>() // Specify the exception type to handle
		  .OrResult<ResponseModel>(r => r.Errors.Any())
		  .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))); // Retry 2 times

		return retryPolicy;
	}
}

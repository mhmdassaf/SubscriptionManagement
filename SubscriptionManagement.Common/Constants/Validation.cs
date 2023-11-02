
namespace SubscriptionManagement.Common.Constants;

public struct Validation
{
	public const string IsRequired = "is required.";
	public const string MustBeAtLeast8Characters = "must be at least 8 characters.";
	public const string SuccessCode = "success";
	public const string SuccessMsg = "Success";

	public struct Auth
	{
		public const string RegisterSuccessMsg = "User registered successfully";
		public const string RegisterSuccessCode = "register_success";

		public const string RegisterFailMsg = "User registeration fail";
		public const string RegisterFailCode = "register_fail";

		public const string LoginSuccessMsg = "login success";
		public const string LoginSucessCode = "login_sucess";

	    public const string UserNotExistOrWrongPasswordMsg = "Wrong user or password";
		public const string UserNotExistOrWrongPasswordCode = "wrong_user_or_password";
	}

	public struct Subscription
	{
		public const string NoActiveSubscriptionFoundMsg = "No active subscription are found";
		public const string NoActiveSubscriptionFoundCode = "no-active-subscription";

		public const string NoSubscriptionFoundMsg = "No subscription are found";
		public const string NoSubscriptionFoundCode = "no-subscription";

	}
}

namespace SubscriptionManagement.Common.AppSettings;

public class AuthSettings
{
    public string? SecretKey { get; set; }
    public string? Issuer { get; set; }
	public string? Audience { get; set; }
	public double Expires { get; set; }
    public bool RequireHttpsMetadata { get; set; }
    public bool ValidateIssuer { get; set; }
    public bool ValidateAudience { get; set; }
}

namespace SubscriptionManagement.DAL.Entities;

public class User : IdentityUser
{
    [MaxLength(10)]
    public string? FirstName { get; set; }

    [MaxLength(10)]
    public string? LastName { get; set; }
}

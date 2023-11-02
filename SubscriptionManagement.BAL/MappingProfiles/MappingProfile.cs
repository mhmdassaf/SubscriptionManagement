using SubscriptionManagement.Common.Models.Subscription;

namespace SubscriptionManagement.BAL.MappingProfiles;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<RegisterDto, User>()
			.ForMember(des => des.UserName, opt => opt.MapFrom(scr => scr.Email))
			.ReverseMap();

		CreateMap<Subscription, SubscriptionModel>().ReverseMap();
	}
}


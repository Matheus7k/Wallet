using AutoMapper;
using Wallet.Application.Commands.v1.Users.AddUser;
using Wallet.Domain.Entities.v1;

namespace Wallet.Application.Profiles.v1;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<AddUserCommand, User>(MemberList.None)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}
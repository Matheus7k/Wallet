using AutoMapper;
using Wallet.Application.Commands.v1.Users.AddUser;
using Wallet.Domain.Entities.v1;
using static BCrypt.Net.BCrypt;

namespace Wallet.Application.Profiles.v1;

public class UserProfile : Profile
{
    private const int WorkFactor = 11;
    
    public UserProfile()
    {
        CreateMap<AddUserCommand, User>(MemberList.None)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => GetHashPassword(src.Password)))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));    
    }

    private static string GetHashPassword(string password) =>
        HashPassword(password, WorkFactor);
}
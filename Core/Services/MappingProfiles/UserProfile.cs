using AutoMapper;
using Domain.Identity;
using Shared.Dto.Auth;

namespace Services.MappingProfiles
{
    internal class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser, UserDto>()
                .ForMember(d => d.Roles, o => o.Ignore())
                .ForMember(d => d.IsLockedOut, o => o.Ignore());

            CreateMap<RegisterDto, AppUser>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.Email));
        }
    }
}
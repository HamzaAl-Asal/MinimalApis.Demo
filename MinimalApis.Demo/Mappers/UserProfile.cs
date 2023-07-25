using AutoMapper;
using MinimalApis.Demo.Context.Models.Users;
using MinimalApis.Demo.Models.Users;

namespace MinimalApis.Demo.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserViewModel>()
                .ReverseMap();
        }
    }
}
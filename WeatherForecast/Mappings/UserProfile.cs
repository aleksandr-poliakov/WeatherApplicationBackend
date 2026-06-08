using AutoMapper;
using WeatherForecast.Dto;
using WeatherForecast.Models;

namespace WeatherForecast.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserCreateDto, User>()
            .ForMember(d => d.PasswordHash, opt => opt.Ignore());
        CreateMap<User, UserResponseDto>();
    }
}
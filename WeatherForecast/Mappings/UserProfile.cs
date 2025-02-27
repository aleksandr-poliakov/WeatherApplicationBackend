using AutoMapper;
using WeatherForecast.Dto;
using WeatherForecast.Models;

namespace WeatherForecast.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserCreateDto, User>();
        CreateMap<User, UserResponseDto>();
    }
}
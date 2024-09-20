using SOPBackend.DTOs;

namespace SOPBackend.MappingProfiles;

using AutoMapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDTO>();  
        CreateMap<UserDTO, User>();  
    }
}

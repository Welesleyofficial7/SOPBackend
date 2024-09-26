using SOPBackend.DTOs;
using SOPBackend.Utils;

namespace SOPBackend.MappingProfiles;

using AutoMapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDTO>();  
        CreateMap<UserDTO, User>();
        CreateMap<Order, OrderDTO>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status)); 

        CreateMap<OrderDTO, Order>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<Status>(src.Status.ToString())));
        
        CreateMap<MenuItem, MenuItemDTO>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category)); 

        CreateMap<MenuItemDTO, MenuItem>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => Enum.Parse<Status>(src.Category.ToString())));

        CreateMap<Order, PlaceOrderDTO>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.PromotionId, opt => opt.MapFrom(src => src.PromotionId))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems));

        CreateMap<PlaceOrderDTO, Order>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.PromotionId, opt => opt.MapFrom(src => src.PromotionId))
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Items));
        
        CreateMap<OrderItem, PlaceOrderItemDTO>()
            .ForMember(dest => dest.MenuItemId, opt => opt.MapFrom(src => src.MenuItemId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src => src.Subtotal));
    }
}

// using SOPBackend.DTOs;

using SOPBackend.DTOs;
using SOPContracts.Dtos;
using SOPBackend.Messages;
using SOPBackend.Utils;

namespace SOPBackend.MappingProfiles;

using AutoMapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponse>();  
        CreateMap<UserRequest, User>();
        CreateMap<Order, OrderResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status)); 

        // CreateMap<OrderRequest, Order>()
        //     .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<Status>(src.Status.ToString())));
        //
        CreateMap<MenuItem, MenuItemResponse>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category)); 
        
        
        CreateMap<MenuItemRequest, MenuItem>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()));

        CreateMap<Order, OrderResponse>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.PromotionId, opt => opt.MapFrom(src => src.PromotionId))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems));

        CreateMap<OrderRequest, Order>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.PromotionId, opt => opt.MapFrom(src => src.PromotionId))
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Items));
        
        CreateMap<OrderItem, OrderItemResponse>()
            .ForMember(dest => dest.MenuItemId, opt => opt.MapFrom(src => src.MenuItemId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src => src.Subtotal));
        
        CreateMap<Order, GetAllOrdersDTO>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.OrderTime, opt => opt.MapFrom(src => src.OrderTime))
            .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.TotalCost));
        
        CreateMap<Promotion, PromotionDTO>()
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate));

        CreateMap<PromotionDTO, Promotion>()
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate));

    }
}

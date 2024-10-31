using AutoMapper;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using SOPBackend.DTOs;
using SOPBackend.Services;
using SOPBackend.Services.Utils;

namespace SOPBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IMenuItemService _menuItemService;
    private readonly IOrderItemService _orderItemService;
    private readonly IPromotionService _promotionService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly IBus _bus;

    public OrderController(IOrderService orderService, IUserService userService, IPromotionService promotionService, IMenuItemService menuItemService, IOrderItemService orderItemService, IMapper iMapper, IBus bus)
    {
        _orderService = orderService;
        _menuItemService = menuItemService;
        _orderItemService = orderItemService;
        _promotionService = promotionService;
        _userService = userService;
        _mapper = iMapper;
        _bus = bus;
    }
    
        [HttpGet("getAll", Name = "GetAllOrders")]
        public IActionResult GetAllOrders()
        {
            var ordersList = _orderService.GetAllOrders().ToList();
            var orders = new List<GetAllOrdersDTO>();
            ordersList.ForEach(c => orders.Add(_mapper.Map<GetAllOrdersDTO>(c)));
            
            return Ok(
                new
                {
                    self = new { href = Url.Link("GetAllOrders", null) },
                    orders,
                    _actions = new
                    {
                        getAll = new
                        {
                            href = Url.Link("GetAllOrders", null),
                            method = "GET",
                            rel = "Find all orders."
                        },
                        getById = new
                        {
                            href = Url.Link("GetOrderById", new { id = "someId" }),
                            method = "GET",
                            rel = "Find an order by Id."
                        },
                        create = new
                        {
                            href = Url.Link("CreateOrder", null),
                            method = "POST",
                            rel = "Create a new order."
                        },
                        update = new
                        {
                            href = Url.Link("UpdateOrder", new { id = "someId" }),
                            method = "PUT",
                            rel = "Update an existing order by ID."
                        },
                        delete = new
                        {
                            href = Url.Link("DeleteOrder", new { id = "someId" }),
                            method = "DELETE",
                            rel = "Delete an order by ID."
                        }
                    },
                }
            );
        }
        
        private async Task PublishNewOrderMessage(Order order)
        {
            var message = order.ToMessage();
            await _bus.PubSub.PublishAsync(message);
        }
        
        private async Task PublishNewUserMessage(User user, Guid id)
        {
            var message = user.ToUserMessage(id);
            await _bus.PubSub.PublishAsync(message);
        }
        
        
        [HttpPut("cancelOrder/{id}", Name="CancelOrder")]
        public async Task<IActionResult> CancelOrder(Guid id)
        {
            var final = _orderService.CancelOrder(id);
            if (final == null)
            {
                return NotFound("Order not found");
            }
            await PublishNewOrderMessage(final);
            return Ok("Order cancelled!");
        }
        
        [HttpPut("startPrepareOrder/{id}", Name="StartPrepareOrder")]
        public async Task<IActionResult> StartPreparingOrder(Guid id)
        {
            var final = _orderService.StartPreparingOrder(id);
            if (final == null)
            {
                return NotFound("Order not found");
            }
            await PublishNewOrderMessage(final);
            return Ok("Start preparing order!");
        }

        [HttpPut("completeOrder/{id}", Name="CompleteOrder")]
        public async Task<IActionResult> CompleteOrder(Guid id)
        {
            var final = _orderService.CompleteOrder(id);
            if (final == null)
            {
                return NotFound("Order not found");
            }
            await PublishNewOrderMessage(final);
            return Ok("Order completed!");
        }
        
        [HttpGet("getById/{id}", Name = "GetOrderById")]
        public IActionResult GetOrderById(Guid id)
        {
            var order = _orderService.GetOrderById(id);
            var orderDTO = _mapper.Map<OrderDTO>(order);
            var orderWithLinks = AddOrderLinks(orderDTO, order.Id);
            return Ok(orderWithLinks);
        }
        
        [HttpPost("create", Name="CreateOrder")]
        public IActionResult CreateUser([FromBody] OrderDTO newOrderDto)
        {
        
            var newOrder = _mapper.Map<Order>(newOrderDto);
        
            var createdOrder = _orderService.CreateOrder(newOrder);
            if (createdOrder == null)
            {
                return BadRequest("Order already exists.");
            }

            var createdOrderDTO = _mapper.Map<OrderDTO>(createdOrder);
            AddOrderLinks(createdOrderDTO, createdOrder.Id);
        
            return CreatedAtRoute("GetOrderById", new { id = createdOrder.Id }, createdOrder);
        }
        
        [HttpPost("placeOrder", Name = "PlaceOrderWithItems")]
        public async Task<IActionResult> PlaceOrderWithItems([FromBody] PlaceOrderDTO placeOrderDto)
        {
            if (placeOrderDto == null || placeOrderDto.Items == null || !(placeOrderDto.Items.Count() > 0))
            {
                return BadRequest("Заказ должен иметь хотя бы один элемент.");
            }
            
            decimal totalCost = 0;
            var orderItems = new List<OrderItem>();

            foreach (var itemDto in placeOrderDto.Items)
            {
                var menuItem = _menuItemService.GetMenuItemById(itemDto.MenuItemId);
                if (menuItem == null)
                {
                    return NotFound($"Menu item with ID {itemDto.MenuItemId} not found.");
                }

                var subtotal = menuItem.Price * itemDto.Quantity;
                totalCost += subtotal;

                var orderItem = new OrderItem(Guid.NewGuid(), itemDto.MenuItemId, itemDto.Quantity, subtotal);
                orderItems.Add(orderItem);
            }
            Console.WriteLine(placeOrderDto.PromotionId);
            if (!placeOrderDto.PromotionId.ToString().Equals(""))
            {
                var sale = _promotionService.GetPromotionById(placeOrderDto.PromotionId);
                if (sale != null && DateTime.Now >= sale.StartDate && DateTime.Now <= sale.EndDate)
                {
                    
                    Console.WriteLine($"Discount applied: {sale.Discount}%");
                    totalCost *= (1 - (sale.Discount / 100));
                    Console.WriteLine($"Total cost after discount: {totalCost}");
                }
            }

            var newOrder = new Order(placeOrderDto.UserId, 0, DateTime.UtcNow, totalCost, placeOrderDto.PromotionId);
            newOrder.OrderItems = orderItems;

            var createdOrder = _orderService.CreateOrder(newOrder);
            
            var user = _userService.GetUserById(placeOrderDto.UserId);
            await PublishNewUserMessage(user, createdOrder.Id);
            
            var final = _orderService.StartPreparingOrder(createdOrder.Id);
            if (final == null)
            {
                return NotFound("Order not found");
            }
            await PublishNewOrderMessage(final);
            
            var final2 = _orderService.ApplyDiscountOrder(createdOrder.Id);
            if (final == null)
            {
                return NotFound("Order not found");
            }
            await PublishNewOrderMessage(final);
            
            var createdOrderDto = _mapper.Map<PlaceOrderDTO>(createdOrder);
    
            var orderWithLinks = AddOrderLinks(createdOrderDto, createdOrder.Id);
            return CreatedAtRoute("GetOrderById", new { id = createdOrder.Id }, orderWithLinks);
        }


        
        [HttpPut("update/{id}", Name="UpdateOrder")]
        public IActionResult UpdateOrder(Guid id, [FromBody] OrderDTO updatedOrderDTO)
        {
            var updatedOrder = _mapper.Map<Order>(updatedOrderDTO);
            var order = _orderService.UpdateOrder(id, updatedOrder);
            var orderDTO = _mapper.Map<OrderDTO>(order);
            var orderWithLinks = AddOrderLinks(orderDTO, updatedOrder.Id);
            return Ok(orderWithLinks);
        }
        
        [HttpDelete("delete/{id}", Name="DeleteOrder")]
        public IActionResult DeleteOrder(Guid id)
        {
            var result = _orderService.DeleteOrder(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
        
        private object AddOrderLinks(OrderDTO order, Guid objId)
        {
            return new
            {
                order,
                _links = new
                {
                    self = new { href = Url.Link("GetOrderById", new { id = objId }) },
                    update = new { href = Url.Link("UpdateOrder", new { id = objId }) },
                    delete = new { href = Url.Link("DeleteOrder", new { id = objId }) },
                    all_users = new { href = Url.Link("GetAllOrders", null) }
                }
            };
        }
        
        private object AddOrderLinks(PlaceOrderDTO order, Guid objId)
        {
            return new
            {
                order,
                _links = new
                {
                    self = new { href = Url.Link("GetOrderById", new { id = objId }) },
                    update = new { href = Url.Link("UpdateOrder", new { id = objId }) },
                    delete = new { href = Url.Link("DeleteOrder", new { id = objId }) },
                    all_users = new { href = Url.Link("GetAllOrders", null) }
                }
            };
        }
        
        
}
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SOPBackend.DTOs;
using SOPBackend.Services;

namespace SOPBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IMenuItemService _menuItemService;
    private readonly IOrderItemService _orderItemService;
    private readonly IPromotionService _promotionService;
    private readonly IMapper _mapper;

    public OrderController(IOrderService orderService, IPromotionService promotionService, IMenuItemService menuItemService, IOrderItemService orderItemService, IMapper iMapper)
    {
        _orderService = orderService;
        _menuItemService = menuItemService;
        _orderItemService = orderItemService;
        _promotionService = promotionService;
        _mapper = iMapper;
    }
    
    [HttpGet("getAll", Name = "GetAllOrders")]
        public IActionResult GetAllOrders()
        {
            var ordersList = _orderService.GetAllOrders().ToList();
            var orders = new List<OrderDTO>();
            ordersList.ForEach(c => orders.Add(_mapper.Map<OrderDTO>(c)));
            
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
        public IActionResult PlaceOrderWithItems([FromBody] PlaceOrderDTO placeOrderDto)
        {
            if (placeOrderDto == null || placeOrderDto.Items == null || !(placeOrderDto.Items.Count() > 0))
            {
                return BadRequest("Order must have at least one item.");
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
                Console.WriteLine(sale.Discount);
                totalCost = totalCost * (1-(sale.Discount / 100));
                Console.WriteLine(totalCost);
            }

            var newOrder = new Order(placeOrderDto.UserId, 0, DateTime.UtcNow, totalCost, placeOrderDto.PromotionId);
            newOrder.OrderItems = orderItems;

            var createdOrder = _orderService.CreateOrder(newOrder);
    
            if (createdOrder == null)
            {
                return BadRequest("Failed to create order.");
            }
            
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
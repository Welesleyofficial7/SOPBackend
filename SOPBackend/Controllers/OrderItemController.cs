using Microsoft.AspNetCore.Mvc;
using SOPBackend.Services;
using System;
using System.Linq;
using AutoMapper;
using SOPBackend.DTOs;
using SOPContracts.Dtos;

namespace SOPBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase, IOrderItemApi
    {
        private readonly IOrderItemService _orderItemService;
        private readonly IMapper _mapper;

        public OrderItemController(IOrderItemService orderItemService, IMapper mapper)
        {
            _orderItemService = orderItemService;
            _mapper = mapper;
        }
        
        [HttpGet("getAll", Name = "GetAllOrderItems")]
        public IActionResult GetAllOrderItems()
        {
            var orderItems = _orderItemService.GetAllOrderItems().ToList();
            var orderItemDtos = new List<OrderItemDTO>();
            orderItems.ForEach(c => orderItemDtos.Add(_mapper.Map<OrderItemDTO>(c)));
    
            return Ok(
                new
                {
                    self = new { href = Url.Link("GetAllOrderItems", null) },
                    orderItemDtos,
                    _actions = new
                    {
                        getAll = new
                        {
                            href = Url.Link("GetAlls", null),
                            method = "GET",
                            rel = "Find all order items."
                        },
                        getById = new
                        {
                            href = Url.Link("GetOrderItemById", new { id = "someId" }),
                            method = "GET",
                            rel = "Find an order item by Id."
                        },
                        create = new
                        {
                            href = Url.Link("CreateOrderItem", null),
                            method = "POST",
                            rel = "Create a new order item."
                        },
                        update = new
                        {
                            href = Url.Link("UpdateOrderItem", new { id = "someId" }),
                            method = "PUT",
                            rel = "Update an existing order item by ID."
                        },
                        delete = new
                        {
                            href = Url.Link("DeleteOrderItem", new { id = "someId" }),
                            method = "DELETE",
                            rel = "Delete an order item by ID."
                        }
                    }
                }
            );
        }

        
        [HttpGet("getById/{id}", Name = "GetOrderItemById")]
        public IActionResult GetOrderItemById(Guid id)
        {
            var orderItem = _orderItemService.GetOrderItemById(id);

            var orderItemWithLinks = AddOrderItemLinks(orderItem);
            return Ok(orderItemWithLinks);
        }
        
        [HttpPost("create", Name="CreateOrderItem")]
        public IActionResult CreateOrderItem([FromBody] OrderItemRequest newOrderItemDto)
        {
        
            var newOrderItem = _mapper.Map<OrderItem>(newOrderItemDto);

            var createdOrderItem = _orderItemService.CreateOrderItem(newOrderItem);
            if (createdOrderItem == null)
            {
                return BadRequest("Order item already exists.");
            }
            
            AddOrderItemLinks(createdOrderItem);
        
            return CreatedAtRoute("GetOrderItemById", new { id = createdOrderItem.Id }, createdOrderItem);
        }

        
        [HttpPut("update/{id}", Name="UpdateOrderItem")]
        public IActionResult UpdateOrderItem(Guid id, [FromBody] OrderItemRequest updatedOrderItemDTO)
        {
            var updatedOrderItem = _mapper.Map<OrderItem>(updatedOrderItemDTO);
            var orderItem = _orderItemService.UpdateOrderItem(id, updatedOrderItem);

            var orderItemWithLinks = AddOrderItemLinks(orderItem);
            return Ok(orderItemWithLinks);
        }
        
        [HttpDelete("delete/{id}", Name="DeleteOrderItem")]
        public IActionResult DeleteOrderItem(Guid id)
        {
            var result = _orderItemService.DeleteOrderItem(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
        
        private object AddOrderItemLinks(OrderItem orderItem)
        {
            return new
            {
                _links = new
                {
                    self = new { href = Url.Link("GetOrderItemById", new { id = orderItem.Id }) },
                    all_order_items = new { href = Url.Link("GetAllOrderItems", null) },
                    create = new { href = Url.Link("CreateOrderItem", null)},
                    update = new { href = Url.Link("UpdateOrderItem", new { id = orderItem.Id }) },
                    delete = new { href = Url.Link("DeleteOrderItem", new { id = orderItem.Id }) }
                }
            };
        }
    }
}

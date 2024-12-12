using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SOPBackend.Controllers;
using SOPBackend.DTOs;
using SOPBackend.Services;
using SOPContracts.Dtos;
using Swashbuckle.AspNetCore.Annotations;

namespace SOPBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemController : ControllerBase, IMenuItemApi
    {
        private readonly IMenuItemService _menuItemService;
        private readonly IMapper _mapper;

        public MenuItemController(IMenuItemService menuItemService, IMapper mapper)
        {
            _menuItemService = menuItemService;
            _mapper = mapper;
        }

        [HttpGet("getAll", Name = "GetAllMenuItems")]
        public IActionResult GetAllMenuItems()
        {
            var menuItems = _menuItemService.GetAllMenuItems().ToList();
            var menuItemDtos = new List<MenuItemDTO>();
            menuItems.ForEach(c => menuItemDtos.Add(_mapper.Map<MenuItemDTO>(c)));

            return Ok(
                new
                {
                    self = new { href = Url.Link("GetAllMenuItems", null) },
                    menuItemDtos,
                    _actions = new
                    {
                        getAll = new
                        {
                            href = Url.Link("GetAllMenuItems", null),
                            method = "GET",
                            rel = "Find all menu items."
                        },
                        getById = new
                        {
                            href = Url.Link("GetMenuItemById", new { id = "someId" }),
                            method = "GET",
                            rel = "Find a menu item by Id."
                        },
                        create = new
                        {
                            href = Url.Link("CreateMenuItem", null),
                            method = "POST",
                            rel = "Create a new menu item."
                        },
                        update = new
                        {
                            href = Url.Link("UpdateMenuItem", new { id = "someId" }),
                            method = "PUT",
                            rel = "Update an existing menu item by ID."
                        },
                        delete = new
                        {
                            href = Url.Link("DeleteMenuItem", new { id = "someId" }),
                            method = "DELETE",
                            rel = "Delete a menu item by ID."
                        }
                    },
                }
            );
        }

        [HttpGet("getById/{id}", Name = "GetMenuItemById")]
        public IActionResult GetMenuItemById(Guid id)
        {
            var menuItem = _menuItemService.GetMenuItemById(id);

            if (menuItem == null)
            {
                return NotFound();
            }

            var menuItemWithLinks = AddMenuItemLinks(_mapper.Map<MenuItemResponse>(menuItem));
            return Ok(menuItemWithLinks);
        }
        
        [HttpPost("create", Name = "CreateMenuItem")]
        public IActionResult CreateMenuItem([FromBody] MenuItemRequest newMenuItemDto)
        {
            var newMenuItem = _mapper.Map<MenuItem>(newMenuItemDto);
            var createdMenuItem = _menuItemService.CreateMenuItem(newMenuItem);

            if (createdMenuItem == null)
            {
                return BadRequest("Menu item already exists.");
            }
            
            var wrappedMenuItem = _mapper.Map<MenuItemResponse>(createdMenuItem);

            AddMenuItemLinks(wrappedMenuItem);

            return CreatedAtRoute("GetMenuItemById", new { id = createdMenuItem.Id }, createdMenuItem);
        }
        
        [HttpPut("update/{id}", Name = "UpdateMenuItem")]
        public IActionResult UpdateMenuItem(Guid id, [FromBody] MenuItemRequest updatedMenuItemDto)
        {
            var updatedMenuItem = _mapper.Map<MenuItem>(updatedMenuItemDto);
            var menuItem = _menuItemService.UpdateMenuItem(id, updatedMenuItem);

            if (menuItem == null)
            {
                return NotFound("Menu item not found.");
            }
            
            var wrappedMenuItem = _mapper.Map<MenuItemResponse>(menuItem);

            var menuItemWithLinks = AddMenuItemLinks(wrappedMenuItem);
            return Ok(menuItemWithLinks);
        }
        
        [HttpDelete("delete/{id}", Name = "DeleteMenuItem")]
        public IActionResult DeleteMenuItem(Guid id)
        {
            var result = _menuItemService.DeleteMenuItem(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        private object AddMenuItemLinks(MenuItemResponse menuItem)
        {
            return new
            {
                menuItem.Name,
                menuItem.Price,
                menuItem.Description,
                menuItem.Category,
                _links = new
                {
                    self = new { href = Url.Link("GetMenuItemById", new { id = menuItem.Id }) },
                    update = new { href = Url.Link("UpdateMenuItem", new { id = menuItem.Id }) },
                    delete = new { href = Url.Link("DeleteMenuItem", new { id = menuItem.Id }) },
                    all_menu_items = new { href = Url.Link("GetAllMenuItems", null) }
                }
            };
        }
    }
}

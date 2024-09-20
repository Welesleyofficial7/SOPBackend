using Microsoft.AspNetCore.Mvc;
using SOPBackend.Services;
using System;
using System.Linq;
using AutoMapper;
using SOPBackend.DTOs;

namespace SOPBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        
        
        [HttpGet("/getAll", Name = "GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers().ToList();
    
            return Ok(new
            {
                users, 
                _links = new
                {
                    self = new { href = Url.Link("GetAllUsers", null) }
                },
                count = users.Count 
            });
        }

        
        [HttpGet("/getById/{id}", Name = "GetUserById")]
        public IActionResult GetUserById(Guid id)
        {
            var user = _userService.GetUserById(id);

            var userWithLinks = AddUserLinks(user);
            return Ok(userWithLinks);
        }
        
        [HttpPost("/create", Name="CreateUser")]
        public IActionResult CreateUser([FromBody] UserDTO newUserDto)
        {
        
            var newUser = _mapper.Map<User>(newUserDto);

            Console.WriteLine(newUser.Email);
            Console.WriteLine("");
        
            var createdUser = _userService.CreateUser(newUser);
            if (createdUser == null)
            {
                return BadRequest("User already exists.");
            }
            
            AddUserLinks(createdUser);
        
            return CreatedAtRoute("GetUserById", new { id = createdUser.Id }, createdUser);
        }

        
        [HttpPut("/update/{id}", Name="UpdateUser")]
        public IActionResult UpdateUser(Guid id, [FromBody] UserDTO updatedUserDTO)
        {
            var updatedUser = _mapper.Map<User>(updatedUserDTO);
            var user = _userService.UpdateUser(id, updatedUser);

            var userWithLinks = AddUserLinks(user);
            return Ok(userWithLinks);
        }
        
        [HttpDelete("{id}", Name="DeleteUser")]
        public IActionResult DeleteUser(Guid id)
        {
            var result = _userService.DeleteUser(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
        
        private object AddUserLinks(User user)
        {
            return new
            {
                user.Name,
                user.Email,
                user.PhoneNumber,
                user.DestinationAddress,
                _links = new
                {
                    self = new { href = Url.Link("GetUserById", new { id = user.Id }) },
                    update = new { href = Url.Link("UpdateUser", new { id = user.Id }) },
                    delete = new { href = Url.Link("DeleteUser", new { id = user.Id }) },
                    all_users = new { href = Url.Link("GetAllUsers", null) }
                }
            };
        }
        
    }
}

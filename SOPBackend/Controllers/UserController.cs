using Microsoft.AspNetCore.Mvc;
using SOPBackend.Services;
using System;
using System.Linq;
using AutoMapper;
using EasyNetQ;
using SOPBackend.DTOs;
using SOPBackend.Services.Utils;
using SOPContracts.Dtos;

namespace SOPBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase, IUserApi
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IBus _bus;

        public UserController(IUserService userService, IMapper mapper, IBus bus)
        {
            _userService = userService;
            _mapper = mapper;
            _bus = bus;
        }
        
        [HttpGet("getAll", Name = "GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers().ToList();
            var userDTOs = new List<UserDTO>();
            users.ForEach(c => userDTOs.Add(_mapper.Map<UserDTO>(c)));
    
            return Ok(
                new
                {
                    self = new { href = Url.Link("GetAllUsers", null) },
                    userDTOs,
                    _actions = new
                    {
                        getAll = new
                        {
                            href = Url.Link("GetAllUsers", null),
                            method = "GET",
                            rel = "Find all users."
                        },
                        getById = new
                        {
                            href = Url.Link("GetUserById", new { id = "someId" }),
                            method = "GET",
                            rel = "Find a user by Id."
                        },
                        create = new
                        {
                            href = Url.Link("CreateUser", null),
                            method = "POST",
                            rel = "Create a new user."
                        },
                        update = new
                        {
                            href = Url.Link("UpdateUser", new { id = "someId" }),
                            method = "PUT",
                            rel = "Update an existing user by ID."
                        },
                        delete = new
                        {
                            href = Url.Link("DeleteUser", new { id = "someId" }),
                            method = "DELETE",
                            rel = "Delete a user by ID."
                        }
                    },
                }
            );
        }

        
        [HttpGet("getById/{id}", Name = "GetUserById")]
        public IActionResult GetUserById(Guid id)
        {
            var user = _userService.GetUserById(id);

            var userWithLinks = AddUserLinks(user);
            return Ok(userWithLinks);
        }
        
        [HttpPost("create", Name="CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest newUserDto)
        {
        
            var newUser = _mapper.Map<User>(newUserDto);

            Console.WriteLine(newUser.Email);
            Console.WriteLine("");
        
            var createdUser = _userService.CreateUser(newUser);
            // await PublishNewUserMessage(newUser);
            var createdUserDTO = _mapper.Map<UserDTO>(createdUser);
            if (createdUser == null)
            {
                return BadRequest("User already exists.");
            }
            
            AddUserLinks(createdUser);
        
            return CreatedAtRoute("GetUserById", new { id = createdUser.Id }, createdUserDTO);
        }

        
        [HttpPut("update/{id}", Name="UpdateUser")]
        public IActionResult UpdateUser(Guid id, [FromBody] UserRequest updatedUserDTO)
        {
            var updatedUser = _mapper.Map<User>(updatedUserDTO);
            var user = _userService.UpdateUser(id, updatedUser);

            var userWithLinks = AddUserLinks(user);
            return Ok(userWithLinks);
        }
        
        [HttpDelete("delete/{id}", Name="DeleteUser")]
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

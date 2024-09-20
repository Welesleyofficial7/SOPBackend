using Microsoft.AspNetCore.Mvc;

namespace SOPBackend.Controllers
{
    [Route("api")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet("/", Name = "Starter")]
        public IActionResult Starter()
        {
            var entity = "users";

            return Ok(
                new
                {
                    self = new { href = Url.Link("Starter", null) },
                    entity,
                    _actions = new
                    {
                        getAll = new
                        {
                            href = Url.Link("GetAllUsers", null),
                            method = "GET",
                            description = "Find all users."
                        },
                        getById = new
                        {
                            href = Url.Link("GetUserById", new { id = "someId" }),
                            method = "GET",
                            description = "Find a user by Id."
                        },
                        create = new
                        {
                            href = Url.Link("CreateUser", null),
                            method = "POST",
                            description = "Create a new user."
                        },
                        update = new
                        {
                            href = Url.Link("UpdateUser", new { id = "someId" }),
                            method = "PUT",
                            description = "Update an existing user by ID."
                        },
                        delete = new
                        {
                            href = Url.Link("DeleteUser", new { id = "someId" }),
                            method = "DELETE",
                            description = "Delete a user by ID."
                        }
                    },
                }
            );
        }
    }
}



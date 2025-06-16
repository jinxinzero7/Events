using EventPlatform.Application.DTO;
using EventPlatform.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventPlatform.WebApi.Controllers
{
    [ApiController]
    [Route("userController")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService service)
        {
            _userService = service;
        }
        /*
        [HttpPost("Register")]
        public IActionResult Post(RegisterRequest req)
        {
            _userService.RegisterAsync(req);
        }
        */
    }
}

// AuthenticationController
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using NetCoreApp.Application.Interfaces.Repositories;
using NetCoreApp.Domain.Entities;
using Peddle.Foundation.Common.Extensions;

namespace NetCoreApp.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AuthenticationController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] User user)
        {
            var existingUser = await _userRepository.GetUserByEmail(user.Email);
            if (existingUser != null)
            {
                return BadRequest("user_already_exist");
            }
            if(user.Email.IsEmpty() || user.Password.IsEmpty() || user.Name.IsEmpty())
            {
                return BadRequest("email_or_password_invalid");
            }
            user.CreatedAt = DateTime.UtcNow;
            await _userRepository.AddUser(user);
            return Ok("User created successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User loginUser)
        {
            var user = await _userRepository.GetUserByEmail(loginUser.Email);
            if (user == null || user.Password != loginUser.Password)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok("Login successful");
        }
    }
}
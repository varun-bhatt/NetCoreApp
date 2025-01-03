// AuthController
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using NetCoreApp.Application.Interfaces.Repositories;
using NetCoreApp.Domain.Entities;
using NetCoreApp.Helpers;
using Peddle.Foundation.Common.Extensions;

namespace NetCoreApp.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("authentication/signup")]
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
            user.Password = EncryptionHelper.Encrypt(user.Password);
            await _userRepository.AddUser(user);
            return Ok("User created successfully");
        }

        [HttpPost("authentication/login")]
        public async Task<IActionResult> Login([FromBody] User loginUser)
        {
            var user = await _userRepository.GetUserByEmail(loginUser.Email);
            if (user == null || user.Password != EncryptionHelper.Decrypt(loginUser.Password))
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok("Login successful");
        }
    }
}
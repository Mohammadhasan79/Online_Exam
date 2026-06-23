using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineExam.DTOs.AuthDTOs;
using OnlineExam.ServiceInterfaces;

namespace OnlineExam.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            var result = await _userService.RegisterAsync(register);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            var result = await _userService.LoginAsync(login);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            var result = await _userService.RefreshToken(refreshToken);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        [HttpGet]
        [Authorize(Roles ="Student")]
        public async Task<IActionResult> Student()
        {
            return Ok("hi");
        }
        [HttpGet("[action]")]
        [Authorize(Roles = "Prof")]
        public async Task<IActionResult> Prof()
        {
            return Ok("hi");
        }
    }
}

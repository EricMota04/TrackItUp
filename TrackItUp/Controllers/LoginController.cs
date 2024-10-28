using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackItUp.Dtos.LoginDtos;
using TrackItUpBLL.Contracts;

namespace TrackItUp.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IUserService userService, ILogger<LoginController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        /// <summary>
        /// public method to login a user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest.Email == null || loginRequest.Password == null)
            {
                return BadRequest("Email and password are required");
            }
            try
            {
                var user = await _userService.VerifyCredentials(loginRequest.Email, loginRequest.Password);
                if (user.Success == false)
                {
                    return Unauthorized(user.Message);
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

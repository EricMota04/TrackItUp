using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackItUp.Dtos.UserDtos;
using TrackItUpBLL.Contracts;

namespace TrackItUp.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService _userService;
        ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) 
        {
            if(id == null) 
            { 
                return BadRequest("The ID is required");
            }
            try
            {
                var user = await _userService.GetById(id);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occurred while creating the user.");
                return StatusCode(500, "Internal server error");
            }

        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddUserDto addUserDto)
        {
            if (addUserDto == null)
            {
                return BadRequest("User data is required.");
            }

            TrackItUpBLL.Dtos.UserDtos.UserAddDto user = new()
            {
                Email = addUserDto.Email,
                Name = addUserDto.Name,
                Password = addUserDto.Password,
            };

            try
            {
                var userCreate = await _userService.AddUser(user);

                if (!userCreate.Success)
                {
                    return BadRequest(userCreate); 
                }

                var uri = Url.Action(nameof(GetById), "User", new { id = userCreate.Data.UserId }, Request.Scheme);
                return Created(uri, userCreate.Data); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the user.");
                return StatusCode(500, "Internal server error"); 
            }
        }

    }
}

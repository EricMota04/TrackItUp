using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackItUp.Dtos.UserDtos;
using TrackItUpBLL.Contracts;
using TrackItUpBLL.Core;
using TrackItUpBLL.Dtos.UserDtos;

namespace TrackItUp.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all the users in the data base
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            if (!users.Success)
            {
                return BadRequest(users);
            }
            if (users.Data == null)
            {
                return NotFound(users);
            }
            return Ok(users);
        }

        /// <summary>
        /// Gets a user by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id) 
        {
            if(id == null) 
            { 
                return BadRequest("The ID is required");
            }
            try
            {
                var user = await _userService.GetById(id);
                if (user.Data == null)
                {
                    return NotFound(user);
                }

                return Ok(user);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occurred while creating the user.");
                return StatusCode(500, "Internal server error");
            }

        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="addUserDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResult) , StatusCodes.Status201Created)]
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

        /// <summary>
        /// To change the password of a user
        /// </summary>
        /// <param name="updateUserDto"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] UpdateUserDto updateUserDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                var userResult = await _userService.GetById(updateUserDto.Id);

                if (userResult.Data == null)
                {
                    return NotFound(); 
                }
                UserUpdateDto userToUpdate = new()
                {
                    Id = updateUserDto.Id,
                    Password = updateUserDto.Password,
                };
                var updateUser = await _userService.UpdateUser(userToUpdate);

                return Ok(updateUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario por ID");
                return StatusCode(500, "Error interno del servidor");
            }

        }

    }
}

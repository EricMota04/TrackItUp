using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackItUp.Dtos.HabitTrackingDtos;
using TrackItUp.Dtos.UserDtos;
using TrackItUpBLL.Contracts;
using TrackItUpBLL.Core;
using TrackItUpBLL.Dtos.HabitTrackingDtos;
using TrackItUpBLL.Dtos.UserDtos;
using TrackItUpBLL.Services;
using TrackItUpDAL.Entities;
using System.Linq;

namespace TrackItUp.Controllers
{
    [Route("api/HabitTracking")]
    [ApiController]
    public class HabitTrackingController : ControllerBase
    {
        private readonly IHabitTrackingService _habitTrackingService;
        private readonly IHabitService _habitService;
        private readonly ILogger<HabitTrackingController> _logger;

        public HabitTrackingController(IHabitTrackingService habitTrackingService, ILogger<HabitTrackingController> logger, IHabitService habitService)
        {
            _habitTrackingService = habitTrackingService;
            _logger = logger;
            _habitService = habitService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            var trackings = await _habitTrackingService.GetAll();
            if (!trackings.Success)
            {
                return BadRequest(trackings);
            }
            if (trackings.Data == null)
            {
                return NotFound();
            }
            return Ok(trackings);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var habitTracking = await _habitTrackingService.GetById(id);

            if (habitTracking.Data == null)
            {
                return NotFound();
            }
            if (!habitTracking.Success)
            {
                return BadRequest(habitTracking);
            }
            return Ok(habitTracking);
        }


        /// <summary>
        /// Retrieves habit trackings by habit ID.
        /// </summary>
        /// <param name="id">The ID of the habit.</param>
        /// <returns>A list of habit trackings.</returns>
        [HttpGet("habit/{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByHabitId(int id)
        {
            try
            {
                var habitTrackings = await _habitTrackingService.GetHabitTrackingsByHabitID(id);

                if (habitTrackings.Data.Count == 0)
                {
                    return NotFound(new { Message = $"No habit trackings found for habit with ID {id}." });
                }

                if (!habitTrackings.Success)
                {
                    return BadRequest(habitTrackings);
                }

                return Ok(habitTrackings);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while fetching habit trackings.", Error = ex.Message });
            }
        }


        [HttpPost]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] AddHabitTrackingDto addHabitTrackingDto)
        {
            if (addHabitTrackingDto == null)
            {
                return BadRequest("HabitTracking data is required.");
            }

            var habitResult = await _habitService.GetById(addHabitTrackingDto.HabitId);
            if (!habitResult.Success || habitResult.Data == null)
            {
                return NotFound("Habit not found.");
            }

            var habitTracking = new HabitTrackingAddDto
            {
                HabitId = habitResult.Data.HabitId, 
                DateTracked = addHabitTrackingDto.DateTracked,
                IsCompleted = addHabitTrackingDto.IsCompleted,
            };

            try
            {
                var result = await _habitTrackingService.AddHabitTracking(habitTracking);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                var uri = Url.Action(nameof(GetById), "HabitTracking", new { id = result.Data.HabitTrackingId }, Request.Scheme);
                return Created(uri, result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the habit tracking.");
                return StatusCode(500, "Internal server error");
            }
        }



        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] UpdateHabitTrackingDto updateHabitTrackingDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var habitTracking = await _habitTrackingService.GetById(updateHabitTrackingDto.HabitTrackingID);

                if (habitTracking.Data == null)
                {
                    return NotFound();
                }
                HabitTrackingUpdateDto habitTrackingToUpdate = new()
                {
                    HabitTrackingID = updateHabitTrackingDto.HabitTrackingID,
                    DateTracked = updateHabitTrackingDto.DateTracked,
                };
                var updateUser = await _habitTrackingService.UpdateHabitTracking(habitTrackingToUpdate);

                return Ok(updateUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el habit Tracking por ID");
                return StatusCode(500, "Error interno del servidor");
            }
        }

    }
}

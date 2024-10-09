using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackItUp.Dtos.HabitDtos;
using TrackItUpBLL.Contracts;
using TrackItUpBLL.Core;
using TrackItUpBLL.Dtos.HabitDtos;

namespace TrackItUp.Controllers
{
    [Route("api/Habits")]
    [ApiController]
    public class HabitController : ControllerBase
    {
        private readonly IHabitService _habitService;
        private readonly ILogger<HabitController> _logger;
        public HabitController(IHabitService habitService, ILogger<HabitController> logger)
        { 
            _habitService = habitService;
            _logger = logger;
        }


        [HttpGet]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            var habits = await _habitService.GetAll();
            if (!habits.Success) 
            { 
                return BadRequest(habits);
            }
            if (habits.Data == null) 
            {
                return NotFound(habits);
            }
            
            return Ok(habits);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(int id) 
        { 
            var habit = await _habitService.GetById(id);

            if(habit.Data == null) 
            { 
                return NotFound(habit);
            }
            if (!habit.Success) 
            { 
                return BadRequest(habit);
            }
            return Ok(habit);
        }

        [HttpGet("user/{id}")]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByUserId(int id)
        {
            try
            {
                var habits = await _habitService.GetHabitsByUserID(id);

                if (habits.Data == null)
                {
                    return NotFound(new { Message = $"No habits found for user with ID {id}." });
                }

                if (!habits.Success)
                {
                    return BadRequest(new { Message = habits.Message });
                }

                return Ok(habits);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while fetching habits.", Error = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] AddHabitDto addHabitDto)
        {
            try
            {
                if (!TimeSpan.TryParse(addHabitDto.ReminderTime, out var reminderTime))
                {
                    return BadRequest("Invalid Reminder Time format. Please use 'hh:mm:ss'.");
                }

                TrackItUpBLL.Dtos.HabitDtos.HabitAddDto habitAddDto = new()
                {
                    HabitName = addHabitDto.HabitName,
                    Description = addHabitDto.Description,
                    Frequency = addHabitDto.Frequency,
                    ReminderTime = reminderTime, // Use the converted TimeSpan
                    StartDate = addHabitDto.StartDate,
                    UserId = addHabitDto.UserId,
                };

                var addDto = await _habitService.AddHabitAsync(habitAddDto);

                if (addDto.Success)
                {
                    var uri = Url.Action(nameof(GetById), "Habit", new { id = addDto.Data.HabitId }, Request.Scheme);
                    return Created(uri, addDto.Data);
                }

                return BadRequest(addDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromBody] DeleteHabitDto deleteHabitDto)
        {
            HabitDeleteDto habitToDelte = new()
            {
                HabitId = deleteHabitDto.HabitId,
            };
            try
            {
                var habitToDelete = await _habitService.GetById(deleteHabitDto.HabitId);

                if (habitToDelete.Data == null)
                {
                    return NotFound(habitToDelete);
                }

                var deleteResult = await _habitService.DeleteHabit(habitToDelte);

                if (deleteResult.Success)
                {
                    return NoContent(); 
                }

                return BadRequest(deleteResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting habit");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServiceResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] UpdateHabitDto updateHabitDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!TimeSpan.TryParse(updateHabitDto.ReminderTime, out var reminderTime))
                {
                    return BadRequest("Invalid Reminder Time format. Please use 'hh:mm:ss'.");
                }

                var verifyUpdate = await _habitService.GetById(updateHabitDto.HabitId);
                if (!verifyUpdate.Success || verifyUpdate.Data == null)
                {
                    return NotFound(verifyUpdate);
                }

                HabitUpdateDto habitToUpdate = new()
                {
                    Description = updateHabitDto.Description,
                    Frequency = updateHabitDto.Frequency,
                    HabitId = updateHabitDto.HabitId,
                    HabitName = updateHabitDto.HabitName,
                    ReminderTime = reminderTime, 
                    StartDate = updateHabitDto.StartDate,
                };

                var update = await _habitService.UpdateHabit(habitToUpdate);

                if (update.Success)
                {
                    return Ok(update);
                }
                else
                {
                    return BadRequest(new { Message = update.Message });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

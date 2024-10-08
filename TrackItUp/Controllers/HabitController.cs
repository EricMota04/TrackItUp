using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackItUp.Dtos.HabitDtos;
using TrackItUpBLL.Contracts;

namespace TrackItUp.Controllers
{
    [Route("api/Habits")]
    [ApiController]
    public class HabitController : ControllerBase
    {
        private readonly IHabitService _habitService;

        public HabitController(IHabitService habitService)
        { 
            _habitService = habitService;
        }


        [HttpGet]
        [AllowAnonymous]
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
        [AllowAnonymous]
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddHabitDto addHabitDto)
        {
            try
            {
                // Convert the reminder time string to TimeSpan
                if (!TimeSpan.TryParse(addHabitDto.ReminderTime, out var reminderTime))
                {
                    return BadRequest("Invalid Reminder Time format. Please use 'hh:mm:ss'.");
                }

                // Create the HabitAddDto
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

                return BadRequest(addDto.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }



    }
}

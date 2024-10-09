using System.ComponentModel.DataAnnotations;

namespace TrackItUp.Dtos.HabitDtos
{
    public class UpdateHabitDto
    {

        [Required(ErrorMessage ="Habit ID is required")]
        public required int HabitId { get; set; }

        [Required(ErrorMessage = "Habit Name is required")]
        public required string HabitName { get; set; }

        public required string Description { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public required DateTime StartDate { get; set; }

        [Required(ErrorMessage ="Frequency is required")]
        public required string Frequency { get; set; }

        [Required(ErrorMessage ="Habit ID is required")]
        public required string ReminderTime { get; set; }
    }
}

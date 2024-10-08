using System.ComponentModel.DataAnnotations;

namespace TrackItUp.Dtos.HabitDtos
{
    public class AddHabitDto
    {
        [Required(ErrorMessage = "The name of the habit is required")]
        public string HabitName { get; set; }

        [Required(ErrorMessage = "The description of the habit is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The start date of the habit is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "The frequency in which the habit will be done is required")]
        public string Frequency { get; set; }

        //[Required(ErrorMessage = "The reminder time is required")]
        public string ReminderTime { get; set; }

        [Required(ErrorMessage = "The User ID is required")]
        public int UserId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace TrackItUp.Dtos.HabitDtos
{
    public class DeleteHabitDto
    {

        [Required(ErrorMessage = "The ID is required")]
        public required int HabitId { get; set; }

    }
}

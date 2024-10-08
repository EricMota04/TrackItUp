using System.ComponentModel.DataAnnotations;

namespace TrackItUp.Dtos.HabitTrackingDtos
{
    public class AddHabitTrackingDto
    {
        [Required(ErrorMessage = "The habit ID is required")]
        public int HabitId { get; set; } // Se requiere para asociar el seguimiento a un hábito específico

        [Required(ErrorMessage = "IsCompleted is required")]
        public bool IsCompleted { get; set; }
        public DateTime? DateTracked { get; set; }
    }
}

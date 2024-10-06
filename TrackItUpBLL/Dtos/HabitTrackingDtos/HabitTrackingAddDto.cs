using System.ComponentModel.DataAnnotations;

namespace TrackItUpBLL.Dtos.HabitTrackingDtos
{
    public class HabitTrackingAddDto
    {
        [Required(ErrorMessage = "The habit ID is required")]
        public int HabitId { get; set; } // Se requiere para asociar el seguimiento a un hábito específico

        [Required(ErrorMessage = "IsCompleted is required")]
        public bool IsCompleted { get; set; } 
        public DateTime? DateTracked { get; set; }
    }
}

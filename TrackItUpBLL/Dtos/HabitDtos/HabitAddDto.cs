using System.ComponentModel.DataAnnotations;

namespace TrackItUpBLL.DTOs
{
    public class HabitAddDto
    {
        [Required(ErrorMessage = "El nombre del hábito es requerido.")]
        public string HabitName { get; set; }

        [Required(ErrorMessage = "La descripción es requerida.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es requerida.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "La frecuencia es requerida.")]
        public string Frequency { get; set; }

        [Required(ErrorMessage = "El tiempo de recordatorio es requerido.")]
        public TimeSpan ReminderTime { get; set; }

        [Required(ErrorMessage = "El ID del usuario es requerido.")]
        public int UserId { get; set; }
    }
}

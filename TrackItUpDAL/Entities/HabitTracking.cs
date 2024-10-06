using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackItUpDAL.Entities
{
    public class HabitTracking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int HabitTrackingId { get; set; } // ID del seguimiento (auto-generado)
        public required int HabitId { get; set; }
        public required Habit Habit { get; set; }
        public DateTime DateTracked { get; set; }
        public required bool IsCompleted { get; set; }

        public HabitTracking() => DateTracked = DateTime.Now;
    }

}

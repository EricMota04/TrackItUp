
namespace TrackItUpDAL.Entities
{
    public class HabitTracking
    {
        public required int HabitTrackingId { get; set; } // ID del seguimiento (auto-generado)
        public required int HabitId { get; set; }
        public required Habit Habit { get; set; }
        public DateTime DateTracked { get; set; }
        public required bool IsCompleted { get; set; }

        public HabitTracking() => DateTracked = DateTime.Now;
    }

}

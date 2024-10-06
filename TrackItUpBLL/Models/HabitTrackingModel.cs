namespace TrackItUpBLL.Models
{
    public class HabitTrackingModel
    {
        public required int HabitTrackingId { get; set; } // ID del seguimiento (auto-generado)
        public required int HabitId { get; set; }
        public required HabitModel Habit { get; set; }
        public DateTime DateTracked { get; set; }
        public required bool IsCompleted { get; set; }

        public HabitTrackingModel() => DateTracked = DateTime.Now;
    }
}

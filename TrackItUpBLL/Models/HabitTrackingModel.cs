namespace TrackItUpBLL.Models
{
    public class HabitTrackingModel
    {
        public int HabitTrackingId { get; set; } // ID del seguimiento (auto-generado)
        public required int HabitId { get; set; }
        public  HabitModel Habit { get; set; }
        public DateTime DateTracked { get; set; }
        public bool IsCompleted { get; set; }

        public HabitTrackingModel() => DateTracked = DateTime.Now;
    }
}

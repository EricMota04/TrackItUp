
namespace TrackItUpDAL.Entities
{
    public class Habit
    {
        public int HabitId { get; set; } // ID único del hábito (auto-generado)
        public required string HabitName { get; set; }
        public required string Description { get; set; } 
        public required DateTime StartDate { get; set; } 
        public required string Frequency { get; set; }
        public required TimeSpan ReminderTime { get; set; } 
        public  DateTime CreatedAt { get; set; } 
        public DateTime? DeactivatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public required int UserId { get; set; }
        public required User User { get; set; }
        public ICollection<HabitTracking> HabitTrackings { get; set; } = new List<HabitTracking>();

        public Habit()
        {
            IsActive = true;
            IsDeleted = false;
            CreatedAt = DateTime.Now;
        }
    }
}

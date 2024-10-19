namespace TrackItUpBLL.Dtos.HabitDtos
{
    public class HabitDto
    {
        public int HabitId { get; set; } // ID único del hábito (auto-generado)
        public string HabitName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public string Frequency { get; set; }
        public TimeSpan? ReminderTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeactivatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int UserId { get; set; }
        public bool IsCompletedToday { get; set; }
    }
}

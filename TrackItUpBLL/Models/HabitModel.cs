namespace TrackItUpBLL.Models
{
    public class HabitModel
    {
        public int HabitId { get; set; }
        public required string HabitName { get; set; }
        public required string Description { get; set; }
        public required DateTime StartDate { get; set; }
        public required string Frequency { get; set; }
        public required TimeSpan ReminderTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeactivatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public required int UserId { get; set; }
        public required UserModel User { get; set; }
        public ICollection<HabitTrackingModel> HabitTrackings { get; set; } = new List<HabitTrackingModel>();

        public HabitModel()
        {
            IsActive = true;
            IsDeleted = false;
            CreatedAt = DateTime.Now;
        }
    }
}

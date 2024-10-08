﻿namespace TrackItUpBLL.Models
{
    public class HabitModel
    {
        public int HabitId { get; set; }
        public  string HabitName { get; set; }
        public  string Description { get; set; } = "No description provided";
        public  DateTime StartDate { get; set; }
        public  string Frequency { get; set; }
        public  TimeSpan? ReminderTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeactivatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int UserId { get; set; }
        public UserModel User { get; set; }
        public ICollection<HabitTrackingModel> HabitTrackings { get; set; } = new List<HabitTrackingModel>();

        public HabitModel()
        {
            IsActive = true;
            IsDeleted = false;
            CreatedAt = DateTime.Now;
        }
    }
}

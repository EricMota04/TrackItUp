﻿
namespace TrackItUpDAL.Entities
{
    public class User
    {
        public int UserId { get; set; } 
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; } 
        public DateTime CreatedAt { get; set; }
        public ICollection<Habit> Habits { get; set; } = new List<Habit>();

        public User() => CreatedAt = DateTime.Now;
    }

}


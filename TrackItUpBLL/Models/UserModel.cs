namespace TrackItUpBLL.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<HabitModel> Habits { get; set; } = new List<HabitModel>();

        public UserModel() => CreatedAt = DateTime.Now;
    }
}

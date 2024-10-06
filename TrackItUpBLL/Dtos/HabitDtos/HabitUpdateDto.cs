namespace TrackItUpBLL.Dtos.HabitDtos
{
    public class HabitUpdateDto
    {
        public required int HabitId { get; set; } 
        public required string HabitName { get; set; } 
        public required string Description { get; set; }
        public required DateTime StartDate { get; set; }
        public required string Frequency { get; set; }
        public required TimeSpan ReminderTime { get; set; }

    }
}

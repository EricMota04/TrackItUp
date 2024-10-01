using TrackItUpDAL.Core;
using TrackItUpDAL.Entities;

namespace TrackItUpDAL.Interfaces
{
    public interface IHabitRepository : IBaseRepository<Habit>
    {
        Task<Habit> DeactivateHabit(Habit habit);
    }
}

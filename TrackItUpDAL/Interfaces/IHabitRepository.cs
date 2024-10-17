using System.Collections.ObjectModel;
using TrackItUpDAL.Core;
using TrackItUpDAL.Entities;

namespace TrackItUpDAL.Interfaces
{
    public interface IHabitRepository : IBaseRepository<Habit>
    {
        Task<Habit> DeactivateHabit(int habitId);
        Task<Habit> ActivateHabit(int habitId);
        Task<IEnumerable<Habit>> GetHabitsByUserID(int  userID); 
    }
}

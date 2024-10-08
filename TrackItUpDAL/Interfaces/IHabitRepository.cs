using System.Collections.ObjectModel;
using TrackItUpDAL.Core;
using TrackItUpDAL.Entities;

namespace TrackItUpDAL.Interfaces
{
    public interface IHabitRepository : IBaseRepository<Habit>
    {
        Task<Habit> DeactivateHabit(Habit habit);
        Task<IEnumerable<Habit>> GetHabitsByUserID(int  userID); 
    }
}

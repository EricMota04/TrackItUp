using System.Linq.Expressions;
using TrackItUpDAL.Core;
using TrackItUpDAL.Entities;

namespace TrackItUpDAL.Interfaces
{
    public interface IHabitTrackingRepository : IBaseRepository<HabitTracking>
    {
        Task<IEnumerable<HabitTracking>> GetHabitTrackingsByHabitId(int habitId);
        Task<bool> IsHabitCompletedToday(int habitId);
    }
}

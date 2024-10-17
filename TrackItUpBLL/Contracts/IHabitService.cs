using TrackItUpBLL.Core;
using TrackItUpBLL.Dtos.HabitDtos;

using TrackItUpBLL.Responses.HabitResponses;

namespace TrackItUpBLL.Contracts
{
    public interface IHabitService : Core.IBaseService
    {
        Task<HabitAddResponse> AddHabitAsync(HabitAddDto habitAddDto);
        Task<HabitUpdateResponse> UpdateHabit(HabitUpdateDto habitUpdateDto);
        Task<HabitDeleteResponse> DeleteHabit(HabitDeleteDto habitDeleteDto);
        Task<ServiceResult> DeactivateHabit(int habitId);
        Task<ServiceResult> ActivateHabit(int habitId);
        Task<ServiceResult> GetHabitsByUserID(int userId);
    }
}

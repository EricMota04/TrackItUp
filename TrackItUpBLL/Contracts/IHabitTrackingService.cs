using TrackItUpBLL.Core;
using TrackItUpBLL.Dtos.HabitTrackingDtos;
using TrackItUpBLL.Dtos.UserDtos;
using TrackItUpBLL.Responses.HabitTrackingResponses;
using TrackItUpBLL.Responses.UserResponses;

namespace TrackItUpBLL.Contracts
{
    public interface IHabitTrackingService : Core.IBaseService
    {
        Task<HabitTrackingAddResponse> AddHabitTracking(HabitTrackingAddDto habitTrackingAddDto);
        Task<HabitTrackingUpdateResponse> UpdateHabitTracking(HabitTrackingUpdateDto habitTrackingUpdateDto);
        Task<ServiceResult> GetHabitTrackingsByHabitID(int id);

    }
}

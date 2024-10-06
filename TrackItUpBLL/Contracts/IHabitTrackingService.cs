using TrackItUpBLL.Dtos.HabitTrackingDtos;
using TrackItUpBLL.Dtos.UserDtos;
using TrackItUpBLL.Responses.HabitTrackingResponses;
using TrackItUpBLL.Responses.UserResponses;

namespace TrackItUpBLL.Contracts
{
    public interface IHabitTrackingService : Core.IBaseService
    {
        HabitTrackingAddResponse AddHabitTracking(HabitTrackingAddDto habitTrackingAddDto);
        HabitTrackingUpdateResponse UpdateHabitTracking(HabitTrackingUpdateDto habitTrackingUpdateDto);

    }
}

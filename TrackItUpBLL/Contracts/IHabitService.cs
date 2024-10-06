using TrackItUpBLL.Dtos.HabitDtos;
using TrackItUpBLL.DTOs;
using TrackItUpBLL.Responses.HabitResponses;

namespace TrackItUpBLL.Contracts
{
    public interface IHabitService : Core.IBaseService
    {
        Task<HabitAddResponse> AddHabitAsync(HabitAddDto habitAddDto);
        Task<HabitUpdateResponse> UpdateHabit(HabitUpdateDto habitUpdateDto);
        Task<HabitDeleteResponse> DeleteHabit(HabitDeleteDto habitDeleteDto);
    }
}

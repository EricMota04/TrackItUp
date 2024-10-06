using TrackItUpBLL.Dtos.HabitDtos;
using TrackItUpBLL.DTOs;
using TrackItUpBLL.Responses.HabitResponses;

namespace TrackItUpBLL.Contracts
{
    public interface IHabitService : Core.IBaseService
    {
        HabitAddResponse AddHabit(HabitAddDto habitAddDto);
        HabitUpdateResponse UpdateHabit(HabitUpdateDto habitUpdateDto);
        HabitDeleteResponse DeleteHabit(HabitDeleteDto habitDeleteDto);

        
    }
}

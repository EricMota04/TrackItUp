using Microsoft.IdentityModel.Tokens;
using TrackItUpBLL.Core;
using TrackItUpBLL.Dtos.HabitDtos;
using TrackItUpDAL.Entities;
using TrackItUpDAL.Interfaces;
using System.Threading.Tasks;


public static class HabitValidations
{
    private static ServiceResult ValidateHabitCommon(HabitAddDto habitAddDto)
    {
        ServiceResult result = new();

        if (habitAddDto.Frequency.IsNullOrEmpty())
        {
            result.Success = false;
            result.Message = "The frequency that the habit is going to be done is required";
            return result;
        }
        if (habitAddDto.HabitName.IsNullOrEmpty())
        {
            result.Success = false;
            result.Message = "The habit name is required";
            return result;
        }
        if (habitAddDto.UserId.ToString().IsNullOrEmpty())
        {
            result.Success = false;
            result.Message = "The User ID is required to match the habit";
            return result;
        }
        if (habitAddDto.ReminderTime.ToString().IsNullOrEmpty())
        {
            result.Success = false;
            result.Message = "The reminder time is necessary";
            return result;
        }

        result.Success = true;
        return result;
    }

    public static async Task<ServiceResult> IsValidHabitToAddAsync(HabitAddDto habitAddDto, IHabitRepository habitRepository)
    {
        ServiceResult result = ValidateHabitCommon(habitAddDto);

        if (result.Success) // Proceed only if common validation passed
        {
            bool habitExists = await habitRepository.ExistsAsync(hb => hb.UserId == habitAddDto.UserId && hb.HabitName == habitAddDto.HabitName && hb.IsActive);
            if (habitExists)
            {
                result.Success = false;
                result.Message = "This Habit already exists for this user";
            }
        }

        return result;
    }

    public static async Task<ServiceResult> IsValidHabitToUpdateAsync(HabitUpdateDto habitUpdateDto, IHabitRepository habitRepository)
    {
        ServiceResult result = new();

        if (habitUpdateDto == null)
        {
            result.Success = false;
            result.Message = "The habit to update is null";
            return result;
        }

        if (!await habitRepository.ExistsAsync(hb => hb.HabitId == habitUpdateDto.HabitId))
        {
            result.Success = false;
            result.Message = "The habit trying to update does not exist";
            return result;
        }

        Habit habit = await habitRepository.GetById(habitUpdateDto.HabitId);

        HabitAddDto habitAddDto = new()
        {
            HabitName = habitUpdateDto.HabitName,
            Frequency = habitUpdateDto.Frequency,
            Description = habitUpdateDto.Description,
            ReminderTime = habitUpdateDto.ReminderTime,
            StartDate = habitUpdateDto.StartDate,
            UserId = habit.UserId 
        };

        result = ValidateHabitCommon(habitAddDto);

        if (result.Success) 
        {
            bool habitExists = await habitRepository.ExistsAsync(hb => hb.HabitName == habitUpdateDto.HabitName && hb.UserId == habit.UserId && hb.HabitId != habitUpdateDto.HabitId);
            if (habitExists)
            {
                result.Success = false;
                result.Message = "A habit with the same name already exists for this user";
            }
        }

        return result;
    }

    public static async Task<ServiceResult> IsValidHabitToDelete(HabitDeleteDto habitDeleteDto, IHabitRepository habitRepository)
    {
        ServiceResult result = new();

        if (habitDeleteDto == null)
        {
            result.Success = false;
            result.Message = "The habit trying to delete is null";
            return result;
        }

        if (!await habitRepository.ExistsAsync(ht => ht.HabitId == habitDeleteDto.HabitId))
        {
            result.Success = false;
            result.Message = "The Habit trying to delete does not exist";
            return result;
        }

        result.Success = true;
        return result;
    }
}

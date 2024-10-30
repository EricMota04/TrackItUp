using Microsoft.Extensions.Logging;
using TrackItUpBLL.Contracts;
using TrackItUpBLL.Core;
using TrackItUpBLL.Dtos.HabitDtos;

using TrackItUpBLL.Models;
using TrackItUpBLL.Responses.HabitResponses;
using TrackItUpDAL.Interfaces;
using TrackItUpDAL.Entities;
using System.Linq;

namespace TrackItUpBLL.Services
{
    public class HabitService : IHabitService
    {
        private readonly IHabitRepository _habitRepository;
        private readonly IHabitTrackingService _habitTrackingService;
        private readonly ILogger<HabitService> _logger;
        public HabitService(IHabitRepository habitRepository, ILogger<HabitService> logger, IHabitTrackingService habitTrackingService)
        {
            _habitRepository = habitRepository;
            _logger = logger;
            _habitTrackingService = habitTrackingService;
        }

        public async Task<ServiceResult> ActivateHabit(int habitId)
        {
            ServiceResult result = new();
            try
            {
                result.Data = await _habitRepository.ActivateHabit(habitId);
                result.Message = "Habit Activated successfully";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result.Message = $"There was an error Activating the habit {ex.Message}";
                result.Success = false;
                throw;
            }
        }

        public async Task<HabitAddResponse> AddHabitAsync(HabitAddDto habitAddDto)
        {
            HabitAddResponse result = new();

            try
            {
                var IsValidHabit = await HabitValidations.IsValidHabitToAddAsync(habitAddDto, _habitRepository);
                if (IsValidHabit.Success)
                {
                    Models.HabitModel habitModel = new()
                    {
                        Description = habitAddDto.Description ?? "No description provided",
                        Frequency = habitAddDto.Frequency,
                        HabitName = habitAddDto.HabitName,
                        ReminderTime = habitAddDto.ReminderTime,
                        StartDate = habitAddDto.StartDate,
                        UserId = habitAddDto.UserId,

                    };

                    Habit habit = new()
                    {
                        Description = habitModel.Description,
                        Frequency = habitModel.Frequency,
                        HabitName = habitModel.HabitName,
                        ReminderTime = habitModel.ReminderTime,
                        StartDate = habitModel.StartDate,
                        UserId = habitModel.UserId,
                    };

                    await _habitRepository.Add(habit);
                    result.Message = "Habit Added Succesfully";
                    result.Data = habit;
                    return result;
                }
                else
                {
                    result.Success = false;
                    result.Message = IsValidHabit.Message;
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result.Success = false;
                result.Message = $"There was an error adding the habit {ex.InnerException}";
                return result;
            }
        }

        public async Task<ServiceResult> DeactivateHabit(int habitId)
        {
            ServiceResult result = new(); 
            try
            {
                result.Data = await _habitRepository.DeactivateHabit(habitId);
                result.Message = "Habit deactivated successfully";
                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                result.Message = $"There was an error deactivating the habit {ex.Message}";
                result.Success = false;
                throw;
            }
        }


        public async Task<HabitDeleteResponse> DeleteHabit(HabitDeleteDto habitDeleteDto)
        {
            HabitDeleteResponse result = new();

            try
            {
                var IsValidDelete = await HabitValidations.IsValidHabitToDelete(habitDeleteDto, _habitRepository);
                if (IsValidDelete.Success)
                {

                    try
                    {
                        await _habitRepository.Delete(habitDeleteDto.HabitId);
                        result.Message = "Habit deleted succesfully";
                        return result;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        throw;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = IsValidDelete.Message;
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result.Success = false;
                result.Message = $"There was an error deleting the habit {ex.Message}";
                return result;
            }
        }

        public async Task<ServiceResult> GetAll()
        {
            ServiceResult result = new();
            try
            {
                var habits = await _habitRepository.GetAll();

                result.Data = habits.Select(h => new HabitDto()
                {
                    UserId = h.UserId,
                    HabitId = h.HabitId,
                    HabitName = h.HabitName,
                    Frequency = h.Frequency,
                    DeletedAt = h.DeletedAt,
                    ReminderTime = h.ReminderTime,
                    CreatedAt = h.CreatedAt,
                    DeactivatedAt = h.DeactivatedAt,
                    Description = h.Description,
                    IsActive = h.IsActive,
                    IsDeleted = h.IsDeleted,
                    StartDate = h.StartDate,
                    IsCompletedToday = _habitTrackingService.IsHabitCompletedToday(h.HabitId).Result

                }).ToList();
                result.Message = "Success";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result.Message = $"There was an error retrieving the habits {ex.Message}";
                result.Success = false ;
                return result;
            }
        }

        public async Task<ServiceResult> GetById(int id)
        {
            ServiceResult result = new();
            try
            {
                var habit = await _habitRepository.GetById(id);
                if (habit != null)
                {
                    HabitDto habitModel = new()
                    {
                        Frequency = habit.Frequency,
                        HabitName = habit.HabitName,
                        HabitId = habit.HabitId,
                        ReminderTime = habit.ReminderTime,
                        UserId = habit.UserId,
                        CreatedAt = habit.CreatedAt,
                        DeactivatedAt = habit.DeactivatedAt,
                        DeletedAt = habit.DeletedAt,
                        Description = habit.Description,
                        IsActive = habit.IsActive,
                        IsDeleted = habit.IsDeleted,
                        StartDate = habit.StartDate,
                        IsCompletedToday = _habitTrackingService.IsHabitCompletedToday(habit.HabitId).Result

                    };
                    result.Data = habitModel;
                    result.Message = "Success";
                    return result;
                }
                else
                {
                    result.Success = false;
                    result.Message = "Habit is null";
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result.Success = false;
                result.Message = $"There was an error retrieving the habit {ex.Message}";
                return result;
            }
        }

        public async Task<ServiceResult> GetHabitsByUserID(int userId)
        {
            ServiceResult result = new ServiceResult();

            try
            {
                var habits = await _habitRepository.GetHabitsByUserID(userId);
                if (habits == null) 
                {
                    result.Success = false;
                    result.Message = "There's no habits for this user";
                    return result;
                }
                
                result.Data = habits.Select(h => new HabitDto
                {
                    HabitId = h.HabitId,
                    HabitName = h.HabitName,
                    Description = h.Description,
                    Frequency = h.Frequency,
                    ReminderTime = h.ReminderTime,
                    StartDate = h.StartDate,
                    UserId = h.UserId,
                    CreatedAt = h.CreatedAt,
                    DeactivatedAt = h.DeactivatedAt,
                    DeletedAt = h.DeletedAt,
                    IsActive = h.IsActive,
                    IsDeleted = h.IsDeleted,
                    IsCompletedToday = _habitTrackingService.IsHabitCompletedToday(h.HabitId).Result
                }); ;
                result.Message = $"Habits [{userId}]";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                result.Success= false;
                result.Message = $"There was an error retrieving the habits: {ex.Message}";
                return result;
                
            }
            
        }

        public async Task<HabitUpdateResponse> UpdateHabit(HabitUpdateDto habitUpdateDto)
        {
            HabitUpdateResponse result = new();

            try
            {
                var IsValidUpdate = await HabitValidations.IsValidHabitToUpdateAsync(habitUpdateDto, _habitRepository);
                if (IsValidUpdate.Success)
                {
                    var habitToUpdate = await _habitRepository.GetById(habitUpdateDto.HabitId);

                    if (habitToUpdate == null)
                    {
                        result.Success = false;
                        result.Message = "Habit not found.";
                        return result;
                    }

                    habitToUpdate.Frequency = habitUpdateDto.Frequency;
                    habitToUpdate.ReminderTime = habitUpdateDto.ReminderTime;
                    habitToUpdate.Description = habitUpdateDto.Description;

                    await _habitRepository.Update(habitToUpdate);

                    result.Data = habitToUpdate;
                    result.Message = "Habit updated successfully.";
                    return result;
                }
                else
                {
                    result.Success = false;
                    result.Message = IsValidUpdate.Message;
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the habit.");
                result.Success = false;
                result.Message = "An error occurred while updating the habit.";
                return result;
            }

            
        }
    }
}

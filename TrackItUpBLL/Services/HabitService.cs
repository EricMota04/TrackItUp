using Microsoft.Extensions.Logging;
using TrackItUpBLL.Contracts;
using TrackItUpBLL.Core;
using TrackItUpBLL.Dtos.HabitDtos;
using TrackItUpBLL.DTOs;
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
        private readonly ILogger<HabitService> _logger;
        public HabitService(IHabitRepository habitRepository, ILogger<HabitService> logger)
        {
            _habitRepository = habitRepository;
            _logger = logger;
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
                throw;
            }
        }

        public async Task<ServiceResult> GetAll()
        {
            ServiceResult result = new();
            try
            {
                var habits = await _habitRepository.GetAll();

                result.Data = habits.Select(h => new Models.HabitModel()
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
                    HabitTrackings = h.HabitTrackings.Select(ht => new Models.HabitTrackingModel
                    {
                        HabitTrackingId = ht.HabitTrackingId,
                        HabitId = ht.HabitId,
                        DateTracked = ht.DateTracked,
                        IsCompleted = ht.IsCompleted,


                    }).ToList()


                }).ToList();
                result.Message = "Success";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
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
                    Models.HabitModel habitModel = new()
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
                        HabitTrackings = habit.HabitTrackings.Select(x =>
                                                new HabitTrackingModel
                                                {
                                                    HabitId = x.HabitId,
                                                    HabitTrackingId = x.HabitTrackingId,
                                                    IsCompleted = x.IsCompleted,
                                                    DateTracked = x.DateTracked,

                                                }).ToList()
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
                throw;
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
                    habitToUpdate.HabitName = habitUpdateDto.HabitName;
                    habitToUpdate.ReminderTime = habitUpdateDto.ReminderTime;

                    await _habitRepository.Update(habitToUpdate);

                    result.Data = habitToUpdate;
                    result.Message = "Habit updated successfully.";
                }
                else
                {
                    result.Success = false;
                    result.Message = IsValidUpdate.Message;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the habit.");
                result.Success = false;
                result.Message = "An error occurred while updating the habit.";
            }

            return result;
        }
    }
}

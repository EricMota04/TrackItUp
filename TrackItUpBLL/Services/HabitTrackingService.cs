using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using TrackItUpBLL.Contracts;
using TrackItUpBLL.Core;
using TrackItUpBLL.Dtos.HabitTrackingDtos;
using TrackItUpBLL.Responses.HabitTrackingResponses;
using TrackItUpBLL.Validations.HabitTrackingValidations;
using TrackItUpDAL.Entities;
using TrackItUpDAL.Interfaces;
using System.Linq;
using Azure.Core;

namespace TrackItUpBLL.Services
{
    public class HabitTrackingService : IHabitTrackingService
    {
        private readonly IHabitTrackingRepository _habitTrackingRepository;
        private readonly IHabitRepository _habitRepository;
        private readonly ILogger<HabitTrackingService> _logger;

        public HabitTrackingService(IHabitTrackingRepository habitTrackingRepository, 
                                    ILogger<HabitTrackingService> logger,
                                    IHabitRepository habitRepository)
        {
            _habitTrackingRepository = habitTrackingRepository;
            _habitRepository = habitRepository;
            _logger = logger;
        }

        public async Task<HabitTrackingAddResponse> AddHabitTracking(HabitTrackingAddDto habitTrackingAddDto)
        {
            HabitTrackingAddResponse result = new();
            try
            {
                var IsValidHabitTracking = await HabitTrackingValidations
                    .IsValidTrackingToAddAsync(habitTrackingAddDto, _habitTrackingRepository);

                if (IsValidHabitTracking.Success)
                {
                    var habit = await _habitRepository.GetById(habitTrackingAddDto.HabitId);
                    Models.HabitTrackingModel habitTrackingModel = new()
                    {
                        HabitId = habitTrackingAddDto.HabitId,
                        IsCompleted = true,
                        DateTracked = habitTrackingAddDto.DateTracked ?? DateTime.Now,
                        Habit = new Models.HabitModel
                        {
                            HabitId = habit.HabitId,
                            HabitName = habit.HabitName,
                            Frequency = habit.Frequency,
                            Description = habit.Description,
                            ReminderTime = habit.ReminderTime,
                            UserId = habit.UserId,
                        }
                    };

                    TrackItUpDAL.Entities.HabitTracking habitTracking = new()
                    {
                        HabitId = habitTrackingModel.HabitId,
                        HabitTrackingId = habitTrackingModel.HabitTrackingId,
                        IsCompleted = habitTrackingModel.IsCompleted,
                        DateTracked = habitTrackingModel.DateTracked,
                        Habit = new TrackItUpDAL.Entities.Habit
                        {
                            HabitId = habitTrackingModel.HabitId,
                            HabitName = habitTrackingModel.Habit.HabitName,
                            Description = habitTrackingModel.Habit.Description,
                            Frequency = habitTrackingModel.Habit.Frequency
                            
                        }
                    };

                    await _habitTrackingRepository.Add(habitTracking);
                    result.Message = "HabitTracking Added successfully";
                    result.Data = habitTrackingModel;
                    return result;
                }
                else
                {
                    result.Message = IsValidHabitTracking.Message;
                    result.Success = IsValidHabitTracking.Success;
                    return result;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result.Success = false;
                result.Message = $"There was an error adding the habitTracking {ex.Message}";
                return result;
            }
        }

        public async Task<ServiceResult> GetAll()
        {
            ServiceResult result = new();
            try
            {
                var trackings = await _habitTrackingRepository.GetAll();
                result.Data = trackings.Select(ht => new Models.HabitTrackingModel()
                {
                    HabitTrackingId = ht.HabitTrackingId,
                    HabitId = ht.HabitId,
                    DateTracked = ht.DateTracked,
                    IsCompleted = ht.IsCompleted,
                    Habit = new Models.HabitModel() 
                    { 
                        HabitId = ht.Habit.HabitId,
                        HabitName = ht.Habit.HabitName,
                        Description = ht.Habit.Description,
                        Frequency = ht.Habit.Frequency,
                    }
                }).ToList();

                result.Message = "Success";
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message);
                result.Success= false;
                result.Message= $"There was an error retrieving the habit trackings{ex.Message}";
                return result;
            }
        }

        public async Task<ServiceResult> GetById(int id)
        {
            ServiceResult result = new();
            try
            {
                var habitTracking = await _habitTrackingRepository.GetById(id);
                if (habitTracking != null)
                {
                    Models.HabitTrackingModel habitTrackingModel = new()
                    {
                        HabitTrackingId = habitTracking.HabitId,
                        HabitId = habitTracking.HabitId,
                        DateTracked = habitTracking.DateTracked,
                        IsCompleted = habitTracking.IsCompleted,
                        Habit = new Models.HabitModel()
                        {
                            HabitId = habitTracking.Habit.HabitId,
                            HabitName = habitTracking.Habit.HabitName,
                            Description= habitTracking.Habit.Description,
                            Frequency= habitTracking.Habit.Frequency,
                        }
                        
                    };
                    result.Data = habitTrackingModel;
                    result.Message = "Success";
                    return result;
                }
                else
                {
                    result.Success = false;
                    result.Message = "Habit tracking is null";
                    return result;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result.Success = false;
                result.Message = $"There was an error retrieving the habit tracking{ex.Message}";
                return result;
            }
        }

        public async Task<HabitTrackingUpdateResponse> UpdateHabitTracking(HabitTrackingUpdateDto habitTrackingUpdateDto)
        {
            HabitTrackingUpdateResponse result = new();
            try
            {
                var IsValidUpdate = await HabitTrackingValidations.IsValidTrackingToUpdateAsync(habitTrackingUpdateDto, _habitTrackingRepository);
                if (IsValidUpdate.Success) 
                {
                    var habitTracking = await _habitTrackingRepository.GetById(habitTrackingUpdateDto.HabitTrackingID);
                    if (habitTracking == null)
                    {
                        result.Success = false;
                        result.Message = "Habit not found";
                        return result;
                    }
                    habitTracking.DateTracked = habitTrackingUpdateDto.DateTracked;

                    await _habitTrackingRepository.Update(habitTracking);

                    result.Data = habitTrackingUpdateDto;
                    result.Message = "HabitTracking Updated successfully";
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
                _logger?.LogError(ex.Message);
                result.Success = false;
                result.Message = $"There was an error updating the habit tracking{ex.Message}";
                return result;
            }
        }
    }
}

using Microsoft.IdentityModel.Tokens;
using TrackItUpBLL.Core;
using TrackItUpBLL.Dtos.HabitTrackingDtos;
using TrackItUpDAL.Entities;
using TrackItUpDAL.Interfaces;
using System.Threading.Tasks;

namespace TrackItUpBLL.Validations.HabitTrackingValidations
{
    public class HabitTrackingValidations
    {
        private static ServiceResult IsValidHabitTracking(HabitTrackingAddDto addDto)
        {
            ServiceResult result = new();

            if (addDto == null)
            {
                result.Success = false;
                result.Message = "The HabitTracking data is null.";
                return result;
            }

            if (addDto.HabitId.ToString().IsNullOrEmpty())
            {
                result.Success = false;
                result.Message = "The Habit ID is required for tracking.";
                return result;
            }

            if (addDto.DateTracked == default)
            {
                result.Success = false;
                result.Message = "The date for tracking is required.";
                return result;
            }

            result.Success = true;
            return result;
        }

        public static async Task<ServiceResult> IsValidTrackingToAddAsync(HabitTrackingAddDto habitTrackingAddDto,
                                                                          IHabitTrackingRepository habitTrackingRepository)
        {
            ServiceResult result = IsValidHabitTracking(habitTrackingAddDto);

            if (result.Success)
            {
                bool trackingExists = await habitTrackingRepository.ExistsAsync(ht => ht.HabitId == habitTrackingAddDto.HabitId &&
                                                                                     ht.DateTracked == habitTrackingAddDto.DateTracked);

                if (trackingExists)
                {
                    result.Success = false;
                    result.Message = "There's already tracking for this habit on the specified date.";
                }
            }

            return result;
        }

        public static async Task<ServiceResult> IsValidTrackingToUpdateAsync(HabitTrackingUpdateDto habitTrackingUpdateDto,
                                                                             IHabitTrackingRepository habitTrackingRepository)
        {
            ServiceResult result = new();

            if (habitTrackingUpdateDto == null)
            {
                result.Success = false;
                result.Message = "The HabitTracking data is null.";
                return result;
            }

            bool trackingExists = await habitTrackingRepository.ExistsAsync(ht => ht.HabitTrackingId == habitTrackingUpdateDto.HabitTrackingID);

            if (!trackingExists)
            {
                result.Success = false;
                result.Message = "The tracking entry does not exist for update.";
                return result;
            }

            result.Success = true;
            return result;
        }

        public static async Task<ServiceResult> IsValidTrackingToDeleteAsync(int habitTrackingId,
                                                                             IHabitTrackingRepository habitTrackingRepository)
        {
            ServiceResult result = new();

            bool trackingExists = await habitTrackingRepository.ExistsAsync(ht => ht.HabitTrackingId == habitTrackingId);

            if (!trackingExists)
            {
                result.Success = false;
                result.Message = "The tracking entry does not exist for deletion.";
                return result;
            }

            result.Success = true;
            return result;
        }
    }
}

﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrackItUpDAL.Context;
using TrackItUpDAL.Entities;
using TrackItUpDAL.Interfaces;

namespace TrackItUpDAL.Repositories
{
    public class HabitTrackingRepository : IHabitTrackingRepository
    {
        private readonly TrackItUpContext _trackItUpContext;
        private readonly ILogger<HabitTrackingRepository> _logger;

        public HabitTrackingRepository(TrackItUpContext context, ILogger<HabitTrackingRepository> logger)
        {
            _trackItUpContext = context;
            _logger = logger;
        }

        public async Task<HabitTracking> Add(HabitTracking entity)
        {
            try
            {
                await _trackItUpContext.HabitTrackings.AddAsync(entity);
                await _trackItUpContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public Task<HabitTracking> Delete(int id)
        {
            throw new NotSupportedException("Deleting habit tracking entries is not supported.");
        }


        public async Task<IEnumerable<HabitTracking>> GetAll()
        {
            try
            {
                return await _trackItUpContext.HabitTrackings.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }

        public async Task<HabitTracking> GetById(int id)
        {
            try
            {
                HabitTracking habitTracking = await _trackItUpContext.HabitTrackings.FindAsync(id);
                return habitTracking;
            }
            catch (Exception ex) {
                _logger?.LogError(ex.Message);
                throw;
            }

        }

        public async Task<HabitTracking> Update(HabitTracking entity)
        {
            try
            {
                HabitTracking habitTrackingToUpdate = await _trackItUpContext.HabitTrackings.FindAsync(entity.HabitTrackingId);
                if (habitTrackingToUpdate == null)
                {
                    _logger.LogWarning("Habit with ID {Id} not found.", entity.HabitId);
                    return null;
                }

                habitTrackingToUpdate.DateTracked = entity.DateTracked;

                await _trackItUpContext.SaveChangesAsync();
                return habitTrackingToUpdate;
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating habitTracking with ID {Id}: {Message}", entity.HabitTrackingId, ex.Message);
                throw;
            }
        }
    }
}

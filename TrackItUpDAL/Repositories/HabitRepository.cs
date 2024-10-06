﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using TrackItUpDAL.Context;
using TrackItUpDAL.Entities;
using TrackItUpDAL.Interfaces;

namespace TrackItUpDAL.Repositories
{
    public class HabitRepository : IHabitRepository
    {
        private readonly TrackItUpContext _trackItUpContext;
        private readonly ILogger<HabitRepository> _logger;

        public HabitRepository(TrackItUpContext trackItUpContext, ILogger<HabitRepository> logger)
        {
            _logger = logger;
            _trackItUpContext = trackItUpContext;
        }

        public async Task<Habit> Add(Habit entity)
        {
            try
            {
                await _trackItUpContext.Habits.AddAsync(entity);
                await _trackItUpContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex) {

                _logger.LogError(ex.Message);
                throw;
            }

        }

        public async Task<Habit> DeactivateHabit(Habit habit)
        {
            var habitToDeactivate = await _trackItUpContext.Habits.FindAsync(habit.HabitId);

            if (habitToDeactivate == null)
            {
                return null; 
            }

            habitToDeactivate.IsActive = false;
            habitToDeactivate.DeactivatedAt = DateTime.UtcNow;

            await _trackItUpContext.SaveChangesAsync();

            return habitToDeactivate;
        }


        public async Task<Habit> Delete(int id)
        {
            try
            {
                var habit = await _trackItUpContext.Habits.FindAsync(id);
                habit.IsDeleted = true;
                habit.DeletedAt = DateTime.UtcNow;
                await _trackItUpContext.SaveChangesAsync();
                return habit;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                throw;
            }
        }

        public async Task<bool> ExistsAsync(Expression<Func<Habit, bool>> expression)
        {
            try
            {
                return await _trackItUpContext.Habits.AnyAsync(expression);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<Habit>> GetAll()
        {
            try
            {
                return await _trackItUpContext.Habits.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                throw;
            }
        }

        public async Task<Habit> GetById(int id)
        {
            try
            {
                var habit = await _trackItUpContext.Habits.FindAsync(id);
                return habit;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                throw;
            }
        }

        public async Task<Habit> Update(Habit entity)
        {
            try
            {
                Habit entityToUpdate = await _trackItUpContext.Habits.FindAsync(entity.HabitId);

                if (entityToUpdate == null)
                {
                    _logger.LogWarning("Habit with ID {Id} not found.", entity.HabitId);
                    return null; 
                }

                
                entityToUpdate.HabitName = entity.HabitName;
                entityToUpdate.Description = entity.Description;
                entityToUpdate.StartDate = entity.StartDate;
                entityToUpdate.Frequency = entity.Frequency;
                entityToUpdate.ReminderTime = entity.ReminderTime;

                
                await _trackItUpContext.SaveChangesAsync();

                
                return entityToUpdate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating habit with ID {Id}: {Message}", entity.HabitId, ex.Message);
                throw; 
            }
        }

    }
}

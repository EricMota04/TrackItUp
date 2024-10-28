using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
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

        public async Task<Habit> ActivateHabit(int habitId)
        {
            try
            {
                var habitToActivate = await _trackItUpContext.Habits.FindAsync(habitId);

                if (habitToActivate == null)
                {
                    return null;
                }

                habitToActivate.IsActive = true;
                

                await _trackItUpContext.SaveChangesAsync();

                return habitToActivate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
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

        public async Task<Habit> DeactivateHabit(int habitId)
        {
            var habitToDeactivate = await _trackItUpContext.Habits.FindAsync(habitId);

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
                return await _trackItUpContext.Habits.Where(x => x.IsDeleted != true).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                throw;
            }
        }

        public async Task<Habit?> GetById(int id)
        {
            try
            {
                var habit = await _trackItUpContext.Habits.AsNoTracking().FirstOrDefaultAsync(h => h.HabitId == id);
                return habit;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<Habit>> GetHabitsByUserID(int userID)
        {
            var habits = await _trackItUpContext.Habits.Where(u => u.UserId.Equals(userID) && u.IsDeleted != true).ToListAsync();

            return habits;

        }

        public async Task<Habit> Update(Habit entity)
        {
            try
            {
                Habit entityToUpdate = await _trackItUpContext.Habits.FindAsync(entity.HabitId);

                if (entityToUpdate == null)
                {
                    _logger.LogWarning($"Habit with ID {entity.HabitId} not found.", entity.HabitId);
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

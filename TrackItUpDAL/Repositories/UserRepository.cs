
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrackItUpDAL.Context;
using TrackItUpDAL.Entities;
using TrackItUpDAL.Interfaces;

namespace TrackItUpDAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TrackItUpContext _trackItUpContext;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(TrackItUpContext context, ILogger<UserRepository> logger)
        {
            _trackItUpContext = context;
            _logger = logger;
        }

        public async Task<User> Add(User entity)
        {
            try
            {
                await _trackItUpContext.Users.AddAsync(entity);
                await _trackItUpContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public Task<User> Delete(int id)
        {
            throw new Exception("Delete user is not supported yet");
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            try
            {
                return await _trackItUpContext.Users.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<User> GetById(int id)
        {
            try
            {
                return await _trackItUpContext.Users.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message);
                throw;
            }
        }

        public async Task<User> Update(User entity)
        {
            try
            {
                User userToUpdate = await _trackItUpContext.Users.FindAsync(entity.UserId);
                if (userToUpdate == null)
                {
                    throw new Exception($"{entity.UserId} is invalid.");
                }
                
                userToUpdate.Password = entity.Password;

                await _trackItUpContext.SaveChangesAsync();
                return userToUpdate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}

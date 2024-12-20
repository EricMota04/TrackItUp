﻿using TrackItUpDAL.Core;
using TrackItUpDAL.Entities;

namespace TrackItUpDAL.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetByEmail(string email);
    }
}

using TrackItUpBLL.Core;
using TrackItUpBLL.Dtos.UserDtos;
using TrackItUpBLL.Responses.UserResponses;

namespace TrackItUpBLL.Contracts
{
    public interface IUserService : Core.IBaseService
    {
        Task<UserAddResponse> AddUser(UserAddDto userAddDto);
        Task<UserUpdateResponse> UpdateUser(UserUpdateDto userUpdateDto);
        Task<ServiceResult> GetUserByEmail(string email);
        Task<ServiceResult> VerifyCredentials(string email, string password);
    }
}

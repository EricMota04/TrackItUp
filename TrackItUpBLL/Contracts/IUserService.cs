using TrackItUpBLL.Dtos.UserDtos;
using TrackItUpBLL.Responses.UserResponses;

namespace TrackItUpBLL.Contracts
{
    public interface IUserService : Core.IBaseService
    {
        UserAddResponse AddUser(UserAddDto userAddDto);
        UserUpdateResponse UpdateUser(UserUpdateDto userUpdateDto);
    }
}

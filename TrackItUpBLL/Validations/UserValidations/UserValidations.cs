using Microsoft.IdentityModel.Tokens;
using TrackItUpBLL.Core;
using TrackItUpBLL.Dtos.UserDtos;
using TrackItUpDAL.Interfaces;
using System.Threading.Tasks;

namespace TrackItUpBLL.Validations.UserValidations
{
    public class UserValidations
    {
        public static ServiceResult ValidUserCommon(UserAddDto userAddDto)
        {
            ServiceResult result = new();

            if (userAddDto == null)
            {
                result.Success = false;
                result.Message = "The user data is null.";
                return result;
            }

            if (userAddDto.Email.IsNullOrEmpty())
            {
                result.Success = false;
                result.Message = "The email is required.";
                return result;
            }

            if (userAddDto.Password.IsNullOrEmpty())
            {
                result.Success = false;
                result.Message = "The password is required.";
                return result;
            }

            if (userAddDto.Name.IsNullOrEmpty())
            {
                result.Success = false;
                result.Message = "The name is required.";
                return result;
            }

            result.Success = true;
            return result;
        }

        public static async Task<ServiceResult> IsValidUserToAddAsync(UserAddDto userAddDto, IUserRepository userRepository)
        {
            ServiceResult result = ValidUserCommon(userAddDto);

            if (result.Success)
            {
               
                if (await userRepository.ExistsAsync(us => us.Email == userAddDto.Email))
                {
                    result.Success = false;
                    result.Message = "This email is already taken.";
                    return result;
                }
            }

            return result;
        }
    }
}

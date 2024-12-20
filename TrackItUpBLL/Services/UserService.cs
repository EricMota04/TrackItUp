﻿using Microsoft.Extensions.Logging;
using TrackItUpBLL.Contracts;
using TrackItUpBLL.Core;
using TrackItUpBLL.Dtos.UserDtos;
using TrackItUpBLL.Responses.UserResponses;
using TrackItUpBLL.Validations.UserValidations;
using TrackItUpDAL.Interfaces;

namespace TrackItUpBLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, 
                        ILogger<UserService> logger)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<UserAddResponse> AddUser(UserAddDto userAddDto)
        {
            UserAddResponse result = new();

            try
            {
                var IsValidUser = await UserValidations
                                                .IsValidUserToAddAsync(userAddDto, _userRepository);
                if (IsValidUser.Success) 
                {
                    Models.UserModel userModel = new()
                    {
                        Name = userAddDto.Name,
                        Email = userAddDto.Email,
                        Password = userAddDto.Password,
                    };

                    TrackItUpDAL.Entities.User user = new()
                    {
                        Name = userModel.Name,
                        Email = userModel.Email,
                        Password = userModel.Password,
                    };

                    await _userRepository.Add(user);
                    result.Data = userModel;
                    result.Message = "User added succesfully";
                    return result;
                }
                else
                {
                    result.Message = IsValidUser.Message;
                    result.Success = false;
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result.Success = false;
                result.Message = $"There was an adding the user{ex.Message}";
                return result;
            }
        }

        public async Task<ServiceResult> GetAll()
        {
            ServiceResult result = new();
            try
            {
                var users = await _userRepository.GetAll();

                result.Data = users.Select(u => new Models.UserModel
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    Password = u.Password,
                });

                result.Message = "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all users.");
                result.Success = false;
                result.Message = "An error occurred while processing your request.";
                return result;
            }

            return result;
        }


        public async Task<ServiceResult> GetById(int id)
        {
            ServiceResult result = new();

            try
            {
                var user = await _userRepository.GetById(id);
                if (user == null) 
                { 
                    result.Success=false;
                    result.Message = "User Does not exist";
                    return result;
                }

                Models.UserModel userModel = new()
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    CreatedAt = user.CreatedAt,
                };
                result.Data = userModel;
                result.Message = "User found successfully";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the user.");
                result.Success = false;
                result.Message = "An error occurred while processing your request.";
                return result;
            }
        }

        public async Task<ServiceResult> GetUserByEmail(string email)
        {
            ServiceResult result = new();
            try
            {
                result.Data = await _userRepository.GetByEmail(email);
                if(result.Data == null)
                {
                    result.Message = "Email not registered";
                    result.Success = false;
                    return result;
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the email.");
                result.Success = false;
                result.Message = "An error occurred while processing your request.";
                return result;
            }
        }

        public async Task<UserUpdateResponse> UpdateUser(UserUpdateDto userUpdateDto)
        {
            UserUpdateResponse result = new();

            try
            {
                var userToUpdate = await _userRepository.GetById(userUpdateDto.Id);

                if (userToUpdate == null)
                { 
                    result.Success = false;
                    result.Message = "User does not exist";
                }
                userToUpdate.Password = userUpdateDto.Password;
                await _userRepository.Update(userToUpdate);

                result.Data = userToUpdate;
                result.Message = "User updated successfully";
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user.");
                result.Success = false;
                result.Message = "An error occurred while processing your request.";
                return result;
            }
        }

        public async Task<ServiceResult> VerifyCredentials(string email, string password)
        {
            ServiceResult result = new();
            try
            {
                ServiceResult userResult = await GetUserByEmail(email);
                if (!userResult.Success)
                {
                    result.Success = false;
                    result.Message = userResult.Message;
                    return result;
                }
                else if (userResult.Data.Password != password)
                {

                    result.Success = false;
                    result.Message = "Invalid password";
                    return result;
                }
                result.Data = userResult.Data;
                result.Message = "Credentials verified successfully";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while verifying the credentials.");
                result.Success = false;
                result.Message = "An error occurred while verifying the credentials.";
                return result;
            }
        }
    }
}

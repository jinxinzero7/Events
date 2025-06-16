using EventPlatform.Application.DTO;
using EventPlatform.Application.Interfaces;
using EventPlatform.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.Services
{
    public class UserService : IUserService
    {
        IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<UserResponse> RegisterAsync(RegisterRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            if (await _userRepository.GetUserByEmailAsync(request.Email) != null)
            {
                throw new InvalidOperationException($"User with email '{request.Email}' already exists.");
            }
            var newUser = new User
            {
                Email = request.Email,
                PasswordHash = request.PasswordHash, // TODO: Hash the password
                Username = request.Username,
                AccountType = request.AccountType
            };

            // Сохранение пользователя в базе данных
            await _userRepository.CreateUserAsync(newUser);
            var userResponse = new UserResponse
            {
                Id = newUser.Id,
                Email = newUser.Email,
                Username = newUser.Username,
                AccountType = request.AccountType
            };

            return userResponse;

        }
        public async Task<UserResponse> LoginAsync(LoginRequest request)
        {
            //проверка пустого req
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            //проверка существования user с email из request
            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
            if (existingUser == null) 
            {
                throw new InvalidOperationException($"User with email '{request.Email}' doesn't exist.");
            }

            // ДОДЕЛАТЬ ХЕШИРОВАНИЕ
            if (existingUser.PasswordHash != request.Password)
            {
                throw new InvalidOperationException("Wrong password.");
            }

            var userResponse = new UserResponse
            {
                Id = existingUser.Id,
                Email = existingUser.Email,
                Username = existingUser.Username,
                AccountType = existingUser.AccountType
            };

            return userResponse;
        }
    }
}

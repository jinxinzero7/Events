using BCrypt.Net;
using EventPlatform.Application.DTO.Requests.Users;
using EventPlatform.Application.DTO.Responses.Events;
using EventPlatform.Application.DTO.Responses.Users;
using EventPlatform.Application.Interfaces;
using EventPlatform.Application.Interfaces.Events;
using EventPlatform.Application.Interfaces.Users;
using EventPlatform.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventPlatform.Application.Services
{
    public class UserService : IUserService
    {
        IUserRepository _userRepository;
        IEventRepository _eventRepository;
        private const int BcryptWorkFactor = 12; // Оптимальный баланс безопасности и производительности
        private readonly INotificationService _notificationService;

        public UserService(IUserRepository userRepository, INotificationService notificationService, IEventRepository eventRepository)
        {
            _userRepository = userRepository;
            _notificationService = notificationService;
            _eventRepository = eventRepository;
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
                PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password, BcryptWorkFactor), // TODO: Hash the password
                Username = request.Username,
                AccountType = request.AccountType,
                
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

            // ДОДЕЛАТЬ ХЕШИРОВАНИЕ (сделано)
            if (!BCrypt.Net.BCrypt.EnhancedVerify(request.Password, existingUser.PasswordHash))
            {
                throw new InvalidOperationException("Wrong password.");
            }

            return new UserResponse
            {
                Id = existingUser.Id,
                Email = existingUser.Email,
                Username = existingUser.Username,
                AccountType = existingUser.AccountType
            };
        }

        public async Task<UserProfileResponse> GetUserProfileAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }
            return new UserProfileResponse
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                BirthDate = user.BirthDate,
                Phone = user.Phone,
                AccountType = user.AccountType
            };
        }

        public async Task<UserProfileResponse> UpdateUserProfileAsync(Guid userId, UpdateProfileRequest request)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            user.Username = request.Username;
            user.BirthDate = request.BirthDate;
            user.Phone = request.Phone;

            await _userRepository.UpdateUserAsync(user);

            return new UserProfileResponse
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                BirthDate = user.BirthDate,
                Phone = user.Phone,
                AccountType = user.AccountType
            };
        }

        public async Task ChangePasswordAsync(Guid userId, ChangePasswordRequest request)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            if (!BCrypt.Net.BCrypt.EnhancedVerify(request.CurrentPassword, user.PasswordHash))
            {
                throw new InvalidOperationException("Invalid current password.");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.NewPassword, BcryptWorkFactor);
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task<IEnumerable<AllUsersResponse>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users.Select(user => new AllUsersResponse
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                AccountType = user.AccountType,
                CreatedAt = user.CreatedAt,
                IsBlocked = user.IsBlocked
            }).ToList();
        }

        public async Task BlockUserAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) throw new ArgumentException("User not found");

            user.IsBlocked = true;
            await _userRepository.UpdateUserAsync(user);

            // уведомление на почту
            await _notificationService.SendEmailNotification(
                user.Email,
                "Ваш аккаунт заблокирован",
                "Ваш аккаунт был заблокирован администратором."
            );
        }

        public async Task UnblockUserAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            user.IsBlocked = false;
            await _userRepository.UpdateUserAsync(user);

            // уведомление на почту
            await _notificationService.SendEmailNotification(
                user.Email,
                "Ваш аккаунт разблокирован",
                "Ваш аккаунт был разблокирован администратором."
            );
        }

        public async Task<OrganizerPublicProfile> GetOrganizerPublicProfileAsync(Guid organizerId)
        {
            var organizer = await _userRepository.GetOrganizerWithEventsAsync(organizerId);
            if (organizer == null)
                throw new ArgumentException("Organizer not found");

            var events = await _eventRepository.GetEventsByOrganizerIdAsync(organizerId);

            return new OrganizerPublicProfile
            {
                Id = organizer.Id,
                Username = organizer.Username,
                Description = organizer.Description,
                Events = events.Select(e => new EventShortResponse
                {
                    Id = e.Id,
                    Title = e.Title,
                    EventTime = e.EventTime
                }).ToList()
            };
        }
    }
}
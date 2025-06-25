using EventPlatform.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventPlatform.Application.Interfaces.Users;
using EventPlatform.Application.DTO.Responses.Users;
using EventPlatform.Application.Interfaces.Events;

namespace EventPlatform.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IEventRepository _eventRepository;

        public UserRepository(ApplicationDbContext dbContext, IEventRepository eventRepository)
        {
            _dbContext = dbContext;
            _eventRepository = eventRepository;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> GetOrganizerWithEventsAsync(Guid organizerId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == organizerId && u.AccountType == AccountType.Organizer);
        }

    }
}

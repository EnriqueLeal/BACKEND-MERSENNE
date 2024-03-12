using API.DataAccess.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.DataAccess.Implement
{
    public class UsersRepository
    {
    }

    public class UserRepository : IUserRepository
    {
        private readonly QuizDbContext _context;

        public UserRepository(QuizDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Users>> GetUsersAsync(int pageNumber, int pageSize)
        {
            int startIndex = (pageNumber - 1) * pageSize;
            return await _context.Users.Skip(startIndex).Take(pageSize).ToListAsync();
        }

        public async Task<Users> GetUserAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task CreateUserAsync(Users user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }

}

using API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API.DataAccess.Interfaces
{

    public interface IUserRepository
    {
        Task<IEnumerable<Users>> GetUsersAsync(int pageNumber, int pageSize);
        Task<Users> GetUserAsync(int id);
        Task CreateUserAsync(Users user);
    }

    public class PropertyRepository : IPropertyRepository
    {
        private readonly QuizDbContext _context;

        public PropertyRepository(QuizDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Properties>> GetPropertiesAsync()
        {
            return await _context.Properties.ToListAsync();
        }

        public async Task<Properties> GetPropertyAsync(int id)
        {
            return await _context.Properties.FindAsync(id);
        }

        public async Task CreatePropertyAsync(Properties property)
        {
            _context.Properties.Add(property);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePropertyAsync(int id, Properties property)
        {
            if (id != property.PropertyID)
            {
                throw new ArgumentException("ID mismatch");
            }

            _context.Entry(property).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePropertyAsync(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property == null)
            {
                throw new KeyNotFoundException("Property not found");
            }

            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();
        }
    }


}

using API.Models;

namespace API.DataAccess.Interfaces
{
    public interface IPropertyRepository
    {
        Task<IEnumerable<Properties>> GetPropertiesAsync();
        Task<Properties> GetPropertyAsync(int id);
        Task CreatePropertyAsync(Properties property);
        Task UpdatePropertyAsync(int id, Properties property);
        Task DeletePropertyAsync(int id);
    }

}

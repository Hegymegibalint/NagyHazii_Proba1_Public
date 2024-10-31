using Nagyhazii_Proba1.Model;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Nagyhazii_Proba1.Application.Services
{
    public interface IPropertyService
    {
        Task<List<Property>> GetAllPropertiesAsync();
        Task<Property> GetPropertyByIdAsync(int id);
        Task AddPropertyAsync(Property property);
        Task UpdatePropertyAsync(Property property);
        Task DeletePropertyAsync(int id);
    }
}

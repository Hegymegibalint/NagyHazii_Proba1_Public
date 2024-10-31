
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nagyhazii_Proba1.Persistence.MsSql;
using Nagyhazii_Proba1.Model;

namespace Nagyhazii_Proba1.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly RealEstateDbContext _context;

        // Konstruktor, amely a DbContext-et injektálja
        public PropertyService(RealEstateDbContext context)
        {
            _context = context;
        }

        // Minden ingatlan listázása
        public async Task<List<Property>> GetAllPropertiesAsync()
        {
            return await _context.Properties.ToListAsync();
        }

        // Ingatlan lekérdezése ID alapján
        public async Task<Property> GetPropertyByIdAsync(int id)
        {
            return await _context.Properties.FindAsync(id);
        }

        // Új ingatlan hozzáadása
        public async Task AddPropertyAsync(Property property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            if (await _context.Properties.AnyAsync(p => p.Id == property.Id))
            {
                throw new InvalidOperationException("A Property with the same ID already exists.");
            }
            _context.Properties.Add(property);
            await _context.SaveChangesAsync();
        }

        // Ingatlan frissítése
        public async Task UpdatePropertyAsync(Property property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            var existingProperty = await _context.Properties.FirstOrDefaultAsync(p => p.Id == property.Id);
            if (existingProperty == null)
            {
                throw new KeyNotFoundException($"Property with ID {property.Id} not found.");
            }

            existingProperty.Address = property.Address;
            existingProperty.District = property.District;
            existingProperty.Rooms = property.Rooms;
            existingProperty.Area = property.Area;
            existingProperty.SellingPrice = property.SellingPrice;
            existingProperty.RentPrice = property.RentPrice;

            await _context.SaveChangesAsync();
        }

        // Ingatlan törlése ID alapján
        public async Task DeletePropertyAsync(int id)
        {
            var property = await _context.Properties.FirstOrDefaultAsync(p => p.Id == id);
            if (property == null)
            {
                throw new KeyNotFoundException($"Property with ID {id} not found.");
            }

            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();
        }
    }
}

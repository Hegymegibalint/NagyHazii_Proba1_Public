using Nagyhazii_Proba1.Application.Services.Nagyhazii_Proba1.Application.Services;
using Nagyhazii_Proba1.Model;
using Nagyhazii_Proba1.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Nagyhazii_Proba1.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly RealEstateDbContext _context;

        public CustomerService(RealEstateDbContext context)
        {
            _context = context;
        }

        // Összes customer lekérdezése
        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        // Customer lekérdezése ID alapján
        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        // Új customer hozzáadása
        public async Task AddCustomerAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }

        // Több customer hozzáadása (XML betöltés)
        public async Task AddCustomersAsync(IEnumerable<Customer> customers)
        {
            foreach (var customer in customers)
            {
                if (customer.IsValid() && customer.IsPreferenceValid()) // Ellenőrzések a valós adatok érdekében
                {
                    _context.Customers.Add(customer);
                }
            }
            await _context.SaveChangesAsync();
        }

        // Customer frissítése
        public async Task UpdateCustomerAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        // Customer törlése ID alapján
        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {id} not found.");
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }

        public List<Customer> LoadCustomersFromXml(string xmlPath)
        {
            var customers = new List<Customer>();

            try
            {
                var xmlDoc = XDocument.Load(xmlPath);
                var customerElements = xmlDoc.Descendants("Customer");

                foreach (var element in customerElements)
                {
                    var customer = new Customer
                    {
                        Name = element.Element("Name")?.Value,
                        MinRooms = int.Parse(element.Element("MinRooms")?.Value ?? "1"),
                        MaxRooms = int.Parse(element.Element("MaxRooms")?.Value ?? "20"),
                        MinArea = double.Parse(element.Element("MinArea")?.Value ?? "0"),
                        MaxArea = double.Parse(element.Element("MaxArea")?.Value ?? "1000"),
                        MinPrice = element.Element("MinPrice") != null ? (decimal?)decimal.Parse(element.Element("MinPrice").Value) : null,
                        MaxPrice = element.Element("MaxPrice") != null ? (decimal?)decimal.Parse(element.Element("MaxPrice").Value) : null,
                        LookingForRent = bool.Parse(element.Element("LookingForRent")?.Value ?? "false"),
                        LookingForPurchase = bool.Parse(element.Element("LookingForPurchase")?.Value ?? "false")
                    };

                    // Preferált kerületek hozzáadása
                    var districtElements = element.Element("DistrictPreferences")?.Elements("District");
                    if (districtElements != null)
                    {
                        foreach (var districtElement in districtElements)
                        {
                            customer.DistrictPreferences.Add(int.Parse(districtElement.Value));
                        }
                    }

                    customers.Add(customer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading the XML file: {ex.Message}");
            }

            return customers;
        }
    }
}

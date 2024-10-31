using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Nagyhazii_Proba1.Persistence.MsSql;
using System.IO;
using System.Threading.Tasks;
using Nagyhazii_Proba1.Application.Services;
using Nagyhazii_Proba1.Application.Services.Nagyhazii_Proba1.Application.Services;

namespace NagyHazii_Proba1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Konfiguráció betöltése appsettings.json fájlból
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Alapkönyvtár beállítása
                .AddJsonFile("appsettings.json") // JSON konfigurációs fájl hozzáadása
                .Build(); // Konfiguráció építése

            // Szolgáltató felépítése, amely tartalmazza a RealEstateDbContext-et és a PropertyService-et
            var serviceProvider = new ServiceCollection()
                .AddDbContext<RealEstateDbContext>(options =>
                    options.UseSqlite(configuration.GetConnectionString("RealEstateDbConnection")))
                .AddScoped<IPropertyService, PropertyService>() // PropertyService regisztrálása
                .AddScoped<ICustomerService, CustomerService>()
                .BuildServiceProvider();

            // A PropertyService példányosítása a szolgáltató segítségével
            var propertyService = serviceProvider.GetService<IPropertyService>();
            var customerService = serviceProvider.GetService<ICustomerService>();

            // Ellenőrzés, hogy a szolgáltatás nem null-e
            if (propertyService == null || customerService == null)
            {
                Console.WriteLine("Failed to initialize PropertyService / CustomerService");
                return;
            }

            // Futtatjuk az alkalmazás fő logikáját
            try
            {
                await RunApplicationAsync(propertyService, customerService);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }

        // Fő alkalmazás logika egy külön metódusban
        private static async Task RunApplicationAsync(IPropertyService propertyService, ICustomerService customerService)
        {
            while (true)
            {
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1. List all properties");
                Console.WriteLine("2. Add a new property");
                Console.WriteLine("3. Update a property");
                Console.WriteLine("4. Delete a property");
                Console.WriteLine("5. Load customers from XML");
                Console.WriteLine("6. Exit");

                var option = Console.ReadLine();

                try
                {
                    switch (option)
                    {
                        case "1":
                            var properties = await propertyService.GetAllPropertiesAsync();
                            foreach (var property in properties)
                            {
                                Console.WriteLine($"{property.Id}: {property.Address}, District: {property.District}, Area: {property.Area} m², Selling Price: {(property.SellingPrice.HasValue ? property.SellingPrice.Value.ToString("C") : "N/A")}, Rent Price: {(property.RentPrice.HasValue ? property.RentPrice.Value.ToString("C") : "N/A")}");
                            }
                            break;

                        case "2":
                            Console.Write("Enter address: ");
                            var address = Console.ReadLine();
                            Console.Write("Enter district: ");
                            var district = int.Parse(Console.ReadLine());
                            Console.Write("Enter number of rooms: ");
                            var rooms = int.Parse(Console.ReadLine());
                            Console.Write("Enter area (m²): ");
                            var area = double.Parse(Console.ReadLine());
                            Console.Write("Enter selling price (or leave empty if not for sale): ");
                            var sellingPriceInput = Console.ReadLine();
                            decimal? sellingPrice = string.IsNullOrWhiteSpace(sellingPriceInput) ? (decimal?)null : decimal.Parse(sellingPriceInput);
                            Console.Write("Enter rent price (or leave empty if not for rent): ");
                            var rentPriceInput = Console.ReadLine();
                            decimal? rentPrice = string.IsNullOrWhiteSpace(rentPriceInput) ? (decimal?)null : decimal.Parse(rentPriceInput);

                            var newProperty = new Nagyhazii_Proba1.Model.Property
                            {
                                Address = address,
                                District = district,
                                Rooms = rooms,
                                Area = area,
                                SellingPrice = sellingPrice,
                                RentPrice = rentPrice
                            };
                            await propertyService.AddPropertyAsync(newProperty);
                            Console.WriteLine("Property added successfully.");
                            break;

                        case "3":
                            Console.Write("Enter the ID of the property to update: ");
                            var updateId = int.Parse(Console.ReadLine());
                            var propertyToUpdate = await propertyService.GetPropertyByIdAsync(updateId);
                            if (propertyToUpdate != null)
                            {
                                Console.Write("Enter new address (leave empty to keep current): ");
                                var newAddress = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(newAddress))
                                {
                                    propertyToUpdate.Address = newAddress;
                                }

                                Console.Write("Enter new district (leave empty to keep current): ");
                                var newDistrictInput = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(newDistrictInput))
                                {
                                    propertyToUpdate.District = int.Parse(newDistrictInput);
                                }

                                Console.Write("Enter new number of rooms (leave empty to keep current): ");
                                var newRoomsInput = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(newRoomsInput))
                                {
                                    propertyToUpdate.Rooms = int.Parse(newRoomsInput);
                                }

                                Console.Write("Enter new area (leave empty to keep current): ");
                                var newAreaInput = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(newAreaInput))
                                {
                                    propertyToUpdate.Area = double.Parse(newAreaInput);
                                }

                                Console.Write("Enter new selling price (leave empty to keep current): ");
                                var newSellingPriceInput = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(newSellingPriceInput))
                                {
                                    propertyToUpdate.SellingPrice = decimal.Parse(newSellingPriceInput);
                                }

                                Console.Write("Enter new rent price (leave empty to keep current): ");
                                var newRentPriceInput = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(newRentPriceInput))
                                {
                                    propertyToUpdate.RentPrice = decimal.Parse(newRentPriceInput);
                                }

                                await propertyService.UpdatePropertyAsync(propertyToUpdate);
                                Console.WriteLine("Property updated successfully.");
                            }
                            else
                            {
                                Console.WriteLine("Property not found.");
                            }
                            break;

                        case "4":
                            Console.Write("Enter the ID of the property to delete: ");
                            var id = int.Parse(Console.ReadLine());
                            await propertyService.DeletePropertyAsync(id);
                            Console.WriteLine("Property deleted successfully.");
                            break;

                        case "5":
                            Console.Write("Enter the path to the XML file: ");
                            var xmlPath = Console.ReadLine();
                            var customers = customerService.LoadCustomersFromXml(xmlPath);
                            await customerService.AddCustomersAsync(customers);
                            Console.WriteLine("Customers loaded successfully from XML.");
                            break;

                        case "6":
                            return;

                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

    }
}



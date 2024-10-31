using Xunit;
using Nagyhazii_Proba1.Application.Services;
using Nagyhazii_Proba1.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;
using Nagyhazii_Proba1.Persistence.MsSql;

namespace Nagyhazii_Proba1.Test
{
    public class PropertyServiceTests
    {
        private readonly IPropertyService _propertyService;
        private readonly RealEstateDbContext _context;

        public PropertyServiceTests()
        {
            var options = new DbContextOptionsBuilder<RealEstateDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Egyedi adatbázis név minden teszthez
                .Options;

            _context = new RealEstateDbContext(options);
            _propertyService = new PropertyService(_context);
        }

        [Fact]
        public async Task AddPropertyAsync_ShouldAddProperty()
        {
            // Arrange
            _context.Database.EnsureDeleted(); // Az adatbázis tisztítása minden teszt előtt
            var property = new Property
            {
                Address = "123 Main St",
                District = 5,
                Rooms = 3,
                Area = 120.5,
                SellingPrice = 300000
            };

            // Act
            await _propertyService.AddPropertyAsync(property);

            // Assert
            var properties = await _propertyService.GetAllPropertiesAsync();
            Assert.Single(properties); // Csak egy ingatlannak kellene lennie
            Assert.Equal("123 Main St", properties.First().Address);
        }

        [Fact]
        public async Task GetAllPropertiesAsync_ShouldReturnAllProperties()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            var property1 = new Property { Address = "123 Main St", District = 5, Rooms = 3, Area = 120.5, SellingPrice = 300000 };
            var property2 = new Property { Address = "456 Elm St", District = 2, Rooms = 2, Area = 85.0, SellingPrice = 200000 };
            await _propertyService.AddPropertyAsync(property1);
            await _propertyService.AddPropertyAsync(property2);

            // Act
            var properties = await _propertyService.GetAllPropertiesAsync();

            // Assert
            Assert.Equal(2, properties.Count);
            Assert.Contains(properties, p => p.Address == "123 Main St");
            Assert.Contains(properties, p => p.Address == "456 Elm St");
        }

        [Fact]
        public async Task GetPropertyByIdAsync_ShouldReturnPropertyIfExists()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            var property = new Property { Address = "789 Maple St", District = 3, Rooms = 4, Area = 150.0, SellingPrice = 500000 };
            await _propertyService.AddPropertyAsync(property);

            // Act
            var retrievedProperty = await _propertyService.GetPropertyByIdAsync(property.Id);

            // Assert
            Assert.NotNull(retrievedProperty);
            Assert.Equal("789 Maple St", retrievedProperty.Address);
        }

        [Fact]
        public async Task GetPropertyByIdAsync_ShouldReturnNullIfNotExists()
        {
            // Arrange
            _context.Database.EnsureDeleted();

            // Act
            var property = await _propertyService.GetPropertyByIdAsync(999);

            // Assert
            Assert.Null(property);
        }

        [Fact]
        public async Task UpdatePropertyAsync_ShouldUpdatePropertyDetails()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            var property = new Property { Address = "789 Maple St", District = 3, Rooms = 4, Area = 150.0, SellingPrice = 500000 };
            await _propertyService.AddPropertyAsync(property);

            // Act
            property.SellingPrice = 450000;
            await _propertyService.UpdatePropertyAsync(property);

            // Assert
            var updatedProperty = await _propertyService.GetPropertyByIdAsync(property.Id);
            Assert.Equal(450000, updatedProperty.SellingPrice);
        }

        [Fact]
        public async Task DeletePropertyAsync_ShouldRemoveProperty()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            var property = new Property { Address = "101 Oak St", District = 1, Rooms = 2, Area = 75.0, SellingPrice = 180000 };
            await _propertyService.AddPropertyAsync(property);

            // Act
            await _propertyService.DeletePropertyAsync(property.Id);

            // Assert
            var properties = await _propertyService.GetAllPropertiesAsync();
            Assert.Empty(properties);
        }

        [Fact]
        public async Task AddPropertyAsync_ShouldNotAddDuplicateId()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            var property1 = new Property { Id = 1, Address = "123 Main St", District = 5, Rooms = 3, Area = 120.5, SellingPrice = 300000 };
            var property2 = new Property { Id = 1, Address = "456 Elm St", District = 2, Rooms = 2, Area = 85.0, SellingPrice = 200000 };
            await _propertyService.AddPropertyAsync(property1);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _propertyService.AddPropertyAsync(property2));
        }

        [Fact]
        public async Task UpdatePropertyAsync_ShouldThrowExceptionIfPropertyDoesNotExist()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            var property = new Property { Id = 999, Address = "Non-existent St", District = 0, Rooms = 1, Area = 50.0, SellingPrice = 100000 };

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _propertyService.UpdatePropertyAsync(property));
        }
        
        [Fact]
        public async Task DeletePropertyAsync_ShouldThrowExceptionIfPropertyDoesNotExist()
        {
            // Arrange
            _context.Database.EnsureDeleted(); // Az adatbázis tisztítása, hogy biztosan üres legyen

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _propertyService.DeletePropertyAsync(999));
        }

        [Fact]
        public async Task GetAllPropertiesAsync_ShouldReturnEmptyListIfNoProperties()
        {
            // Arrange
            _context.Database.EnsureDeleted();

            // Act
            var properties = await _propertyService.GetAllPropertiesAsync();

            // Assert
            Assert.Empty(properties);
        }
    }
}
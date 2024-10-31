using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Nagyhazii_Proba1.Persistence.MsSql;
using System.IO;

namespace Nagyhazii_Proba1.Persistence.MsSql
{
    public class RealEstateDbContextFactory : IDesignTimeDbContextFactory<RealEstateDbContext>
    {
        public RealEstateDbContext CreateDbContext(string[] args)
        {
            // Konfiguráció betöltése appsettings.json fájlból
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Létrehozzuk a DbContextOptions-t SQLite használatával
            var builder = new DbContextOptionsBuilder<RealEstateDbContext>();
            builder.UseSqlite(configuration.GetConnectionString("RealEstateDbConnection"));

            // Visszaadjuk a RealEstateDbContext példányt
            return new RealEstateDbContext(builder.Options);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorApp.Models
{
    public static class SeedData
    {

        #region Dummy Data
        public static readonly string[] DummyCompanyNames = new[]
        {
            "Company 1", "Company 2", "Company 3", "Company 4", "Company 5", "Company 6", "Company 7", "Company 8", "Company 9", "Company 10"
        };

        public static readonly string[] DummyContactNames = new[]
        {
            "Name 1", "Name 2", "Name 3", "Name 4", "Name 5", "Name 6", "Name 7", "Name 8", "Name 9", "Name 10"
        };

        public static readonly string[] DummyAddresses= new[]
        {
            "Address 1", "Address 2", "Address 3", "Address 4", "Address 5", "Address 6", "Address 7", "Address 8", "Address 9", "Address 10"
        };

        public static readonly string[] DummyCities = new[]
        {
            "City 1", "City 2", "City 3", "City 4", "City 5"
        };

        public static readonly string[] DummyRegions= new[]
{
            "Region 1", "Region 2", "Region 3", "Region 4", "Region 5"

        };
        public static readonly string[] DummyPostalCodes= new[]
        {
            "Postal 1", "Postal 2", "Postal 3", "Postal 4", "Postal 5"

        };
        public static readonly string[] DummyCountries= new[]
        {
            "Country 1", "Country 2", "Country 3", "Country 4", "Country 5"

        };
        public static readonly string[] DummyPhones = new[]
{
            "0123456789", "1234567890", "2345678901", "3456789012", "4567890123"

        };
        #endregion

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new CustomersDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<CustomersDbContext>>()))
            {

                // Look for any customers.
                if (context.Customers.Any())
                {
                    return;   // DB has been seeded
                }

                SeedContext(context);
            }
        }

        private static void SeedContext(CustomersDbContext context, int rowsCount = 15) {

            var rng = new Random();

            context.Customers.AddRange(Enumerable.Range(1, rowsCount).Select(index => new Customer
            {
                CompanyName = DummyCompanyNames[rng.Next(DummyCompanyNames.Length)],
                ContactName = DummyContactNames[rng.Next(DummyContactNames.Length)],
                Address = DummyAddresses[rng.Next(DummyAddresses.Length)],
                City = DummyCities[rng.Next(DummyCities.Length)],
                Region = DummyRegions[rng.Next(DummyRegions.Length)],
                PostalCode = DummyPostalCodes[rng.Next(DummyPostalCodes.Length)],
                Country = DummyCountries[rng.Next(DummyCountries.Length)],
                Phone = DummyPhones[rng.Next(DummyPhones.Length)]
            }).ToArray());

            context.SaveChanges();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using BlazorApp.Models;
using BlazorApp.Data;
using System.Linq;

namespace BlazorAppTest
{
    public class DummyDataDbInitializer
    {
        public DummyDataDbInitializer()
        {
        }

        public void Seed(CustomersDbContext context, int rowsCount)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Seed with fixed data
            context.Customers.AddRange(Enumerable.Range(1, rowsCount).Select(index => new Customer
            {

                CompanyName = SeedData.DummyCompanyNames[index - 1],
                ContactName = SeedData.DummyContactNames[index - 1],
                Address = SeedData.DummyAddresses[index - 1],
                City = SeedData.DummyCities[index - 1],
                Region = SeedData.DummyRegions[index - 1],
                PostalCode = SeedData.DummyPostalCodes[index - 1],
                Country = SeedData.DummyCountries[index - 1],
                Phone = SeedData.DummyPhones[index - 1],
            }).ToArray());

            context.SaveChanges();

            // Seed with random data
            // SeedData.SeedContext(context, rowsCount);
        }
    }
   
}

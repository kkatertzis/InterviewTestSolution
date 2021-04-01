using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlazorApp.Data;
using BlazorApp.Models;

namespace BlazorAppIntegrationTests
{
    public class Utilities
    {
        public static void InitializeDbForTests(CustomersDbContext db)
        {
            Seed(db);
        }

        public static void ReinitializeDbForTests(CustomersDbContext db)
        {
            db.Customers.RemoveRange(db.Customers);
            InitializeDbForTests(db);
        }

        private static void Seed(CustomersDbContext context)
        {
            // Seed with fixed data
            context.Customers.AddRange(Enumerable.Range(1, 5).Select(index => new Customer
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

        }
    }
}


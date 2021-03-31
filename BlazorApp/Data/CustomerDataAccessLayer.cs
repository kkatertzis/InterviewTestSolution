using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp.Models;
using BlazorApp.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Data
{
    public class CustomerDataAccessLayer : ICustomers
    {

        private readonly CustomersDbContext _dbContext;

        public string Token { get; set; }

        public CustomerDataAccessLayer(CustomersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

       public async Task<IEnumerable<Customer>> ListCustomers()
        {
            return await _dbContext.Customers.ToListAsync();
        }

        public async Task<PagedResult<Customer>> ListPaged(int page, int pageSize)
        {
            return await _dbContext.Customers.GetPagedAsync(page, pageSize);
        }

        public async Task<Customer> GetCustomer(int? id)
        {
            return await _dbContext.Customers.FirstOrDefaultAsync(b => b.Id.Equals(id));
        }

        public async Task<Customer> GetCustomerNoTracking(int? id)
        {
            return await _dbContext.Customers.AsNoTracking().FirstOrDefaultAsync(b => b.Id.Equals(id));
        }

        public async Task CreateCustomer(Customer customer)
        {
            _dbContext.Add(customer);
           
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
        }

        public async Task UpdateCustomer(Customer customer)
        {
            _dbContext.Update(customer);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
        }

        public async Task DeleteCustomer(Customer customer)
        {
            await DeleteCustomer(customer.Id);
        }

        public async Task DeleteCustomer(int? id)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(b => b.Id.Equals(id));
            if (customer == null)
            {
                return;
            }

            _dbContext.Customers.Remove(customer);
            await _dbContext.SaveChangesAsync();
        }

    }
}

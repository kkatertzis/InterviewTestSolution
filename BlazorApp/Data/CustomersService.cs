using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp.Shared;
using BlazorApp.Models;

namespace BlazorApp.Data
{
    public class CustomersService 
    {
        private readonly ICustomers customers;

        public CustomersService(ICustomers _objCustomers)
        {
            customers = _objCustomers;
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return await customers.ListCustomers();
        }

        public async Task<PagedResult<Customer>> GetCustomersPaged(int page, int pageSize)
        {
            return await customers.ListPaged(page, pageSize);
        }

        public async Task Create(Customer customer)
        {
           await customers.CreateCustomer(customer);
        }

        public async Task<Customer> Details(int id)
        {
            return await customers.GetCustomer(id);
        }

        public async Task Edit(Customer customer)
        {
            await customers.UpdateCustomer(customer);
        }

        public async Task Delete(int id)
        {
            await customers.DeleteCustomer(id);
        }
    } 
}
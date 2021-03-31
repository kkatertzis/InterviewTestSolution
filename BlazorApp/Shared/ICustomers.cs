using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp.Models;

namespace BlazorApp.Shared
{
    public interface ICustomers
    {
        Task<IEnumerable<Customer>> ListCustomers();
        Task<PagedResult<Customer>> ListPaged(int page, int pageSize);
        Task<Customer> GetCustomer(int? id);
        Task<Customer> GetCustomerNoTracking(int? id);
        Task CreateCustomer(Customer customer);
        Task UpdateCustomer(Customer customer);
        Task DeleteCustomer(Customer customer);
        Task DeleteCustomer(int? id);
        string Token { get; set; }
    }
}

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorApp.Models;
using BlazorApp.Data;
using BlazorApp.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace BlazorApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomers customers;

        public CustomersController(ICustomers _objCustomers)
        {
            customers = _objCustomers;
        }

        // GET: api/<controller>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Customer>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IEnumerable<Customer>> Get()
        public async Task<IActionResult> Get()
        {
            try
            {
                var customersList = await customers.ListCustomers();
                if (customersList == null)
                {
                    return NotFound();
                }

                return Ok(customersList);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // GET: api/<controller>
        [HttpGet]
        [Route("paged")]
        public async Task<PagedResult<Customer>> GetPaged(int page = 1, int pageSize = 10)
        {
            return await customers.ListPaged(page, pageSize);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int? id)
        {

            if (id == null) {
                return BadRequest();
            }

            try
            {
                var customer = await customers.GetCustomer(id);

                if (customer == null)
                {
                    return NotFound(new { Id = id, error = "There was no customer with an id of " + id + "." });
                }
                else
                {
                    return Ok(customer);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }           
           
        }

        // POST api/<controller>
        [HttpPost]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] Customer customer)
        {
            try
            {
                await customers.CreateCustomer(customer);
                return Ok(customer); //StatusCode(StatusCodes.Status201Created); 
            }
            catch (DbUpdateException)
            {
                return BadRequest(); // StatusCode(StatusCodes.Status400BadRequest); 
            }
        }

        // PUT api/<controller>/
        [HttpPut]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromBody] Customer customer)
        {
            try
            {
                var dbCustomer = await customers.GetCustomerNoTracking(customer.Id);
                
                if (dbCustomer == null)
                {
                    return NotFound(new { Id = customer.Id, error = "No customer with an id of " + customer.Id + " to update." });
                }

                await customers.UpdateCustomer(customer);
                return Ok(customer);
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }
            
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            try {
                var dbCustomer = await customers.GetCustomerNoTracking(id);

                if (dbCustomer == null)
                {
                    return NotFound(new { Id = id, error = "No customer with an id of " + id + " to delete." });
                }

                await customers.DeleteCustomer(id);
                return Ok();
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }
           
        }
       
    }
}

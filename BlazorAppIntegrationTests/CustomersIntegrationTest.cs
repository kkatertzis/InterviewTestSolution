using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BlazorApp.Models;
using Xunit;
using BlazorApp;
using Microsoft.AspNetCore.Mvc.Testing;
using BlazorApp.Shared;

namespace BlazorAppIntegrationTests
{
    public class CustomersIntegrationTest
        : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;

        public CustomersIntegrationTest(CustomWebApplicationFactory<Startup> _factory)
        {
            factory = _factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Index")]
        [InlineData("/Counter")]
        [InlineData("/Customers")]
        [InlineData("/editcustomer")]
        public async Task TestClient_Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task TestClient_GetCustomersAsync()
        {
            // Arrange
            var client = factory.CreateClient();
            var request = "/api/customers";

            // Act
            var response = await client.GetAsync(request);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
             
            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task TestClient_GetCustomersPagedAsync()
        {
            // Arrange
            var client = factory.CreateClient();
            var pagesize = 3;
            var request = "/api/customers/paged?page=1&pagesize=" + pagesize;

            // Act
            var response = await client.GetAsync(request);
            var jsonFromGetResponse = await response.Content.ReadAsStringAsync();
            var objPagedCustomers = JsonConvert.DeserializeObject<PagedResult<Customer>>(jsonFromGetResponse);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.IsType<PagedResult<Customer>>(objPagedCustomers);
            Assert.Equal(pagesize, objPagedCustomers.Results.Count);
        }

        [Fact]
        public async Task TestClient_GetCustomerDetailsAsync()
        {
            // Arrange
            var request = "/api/customers/1";
            var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync(request);
            var jsonFromGetResponse = await response.Content.ReadAsStringAsync();
            var objCustomer = JsonConvert.DeserializeObject<Customer>(jsonFromGetResponse);

            Assert.IsType<Customer>(objCustomer);
            Assert.Equal(SeedData.DummyContactNames[0], objCustomer.ContactName);
        }

        
        [Fact]
        public async Task TestClient_PostCustomerAsync()
        {       
            var client = factory.CreateClient();

            // Arrange
            var request = new
            {
                Url = "/api/customers",
                Body = new
                {
                    CompanyName = "Company Name Integration test through api",
                    ContactName = "Contact Name api",
                    Address = "Address api",
                    City = "City api",
                    Region = "Region api",
                    PostalCode = "Postal api",
                    Country = "Country api",
                    Phone = "1234567890"                
                }
            };

            // Act
            var response = await client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        
        [Fact]
        public async Task TestClient_PutCustomerAsync()
        {
            // Arrange
            var client = factory.CreateClient();
            var request = new
            {
                Url = "/api/customers",
                Body = new
                {
                    Id = 2, // use id=2 to edit; id=1 is used to match GET results; leave intact
                    CompanyName = "Company Name edited through api",
                    ContactName = "Contact Name edited api",
                    Address = "Address api",
                    City = "City api",
                    Region = "Region api",
                    PostalCode = "Postal api",
                    Country = "Country api",
                    Phone = "1234567890"
                }
            };

            // Act
            var response = await client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        
        [Fact]
        public async Task TestClient_DeleteCustomerAsync()
        {
            // Arrange
            var client = factory.CreateClient();

            var postRequest = new
            {
                Url = "/api/customers",
                Body = new
                {
                    CompanyName = "Company Name through api",
                    ContactName = "Contact Name api",
                    Address = "Address api",
                    City = "City api",
                    Region = "Region api",
                    PostalCode = "Postal api",
                    Country = "Country api",
                    Phone = "1234567890"
                }
            };

            // Act
            var postResponse = await client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));
            var jsonFromPostResponse = await postResponse.Content.ReadAsStringAsync();
            var objCustomer = JsonConvert.DeserializeObject<Customer>(jsonFromPostResponse);

            var deleteResponse = await client.DeleteAsync(string.Format("/api/customers/{0}", objCustomer.Id));

            // Assert
            postResponse.EnsureSuccessStatusCode();

            Assert.Equal(System.Net.HttpStatusCode.OK, deleteResponse.StatusCode);
        }
    }
}

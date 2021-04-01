using System;
using Xunit;
using BlazorApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using BlazorApp.Controllers;
using BlazorApp.Models;
using FluentAssertions;
using System.Net;
using System.Collections.Generic;
using BlazorApp.Shared;

namespace BlazorAppTest
{
    public class CustomersUnitTest
    {
        private CustomerDataAccessLayer customersDAL;
        public static DbContextOptions<CustomersDbContext> dbContextOptions { get; }
        public static string connectionString = "Server=(localdb)\\mssqllocaldb;Database=CustomerTestDb;Trusted_Connection=True;MultipleActiveResultSets=true";

        private int seedRowsCount = 5;
        private static readonly string testCompanyName = "Test Company 1";
        private static readonly string testContactName = "Test Name 1";
        private static readonly string testAddress = "Test Address 1";
        private static readonly string testCity = "Test City 1";
        private static readonly string testRegion = "Test Region 1";
        private static readonly string testPostalCode = "Test Postal Code 1";
        private static readonly string testCountry = "Test Country 1";
        private static readonly string testPhone = "1234567890";

        private Customer objTestCustomer = TestCustomer();

        private static Customer TestCustomer()
        {
            return new Customer()
            {
                CompanyName = testCompanyName,
                ContactName = testContactName,
                Address = testAddress,
                City = testCity,
                Region = testRegion,
                PostalCode = testPostalCode,
                Country = testCountry,
                Phone = testPhone
            };
        }

        static CustomersUnitTest()
        {
            dbContextOptions = new DbContextOptionsBuilder<CustomersDbContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        public CustomersUnitTest()
        {
            var context = new CustomersDbContext(dbContextOptions);
            DummyDataDbInitializer db = new DummyDataDbInitializer();
            db.Seed(context, seedRowsCount);

            customersDAL = new CustomerDataAccessLayer(context);

        }

        #region list
        [Fact]
        public async void CustomerController_GetCustomers_ReturnOkResult()
        {
            //Arrange  
            var controller = new CustomersController(customersDAL);

            //Act  
            var data = await controller.Get();

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void CustomerController_GetCustomers_ReturnBadRequestResult()
        {
            //Arrange  
            var controller = new CustomersController(customersDAL);
            controller.ModelState.AddModelError("CompanyName", "Required");

            //Act  
            var data = await controller.Get();

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void CustomerController_GetCustomers_MatchResult()
        {
            //Arrange  
            var controller = new CustomersController(customersDAL);

            //Act  
            var data = await controller.Get();

            //Assert  
            var okObjectResult = data as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var modelList = okObjectResult.Value as List<Customer>;
            Assert.NotNull(modelList);

            Assert.IsType<OkObjectResult>(data);

            Assert.Equal(SeedData.DummyCompanyNames[0], modelList[0].CompanyName);
            Assert.Equal(SeedData.DummyContactNames[0], modelList[0].ContactName);
            Assert.Equal(SeedData.DummyAddresses[0], modelList[0].Address);
            Assert.Equal(SeedData.DummyCities[0], modelList[0].City);
            Assert.Equal(SeedData.DummyRegions[0], modelList[0].Region);
            Assert.Equal(SeedData.DummyPostalCodes[0], modelList[0].PostalCode);
            Assert.Equal(SeedData.DummyCountries[0], modelList[0].Country);
            Assert.Equal(SeedData.DummyPhones[0], modelList[0].Phone);
        }

        #endregion

        #region list paged
        [Fact]
        public async void CustomerController_GetCustomersPaged_ReturnOkResult()
        {
            //Arrange  
            var controller = new CustomersController(customersDAL);

            //Act  
            var data = await controller.GetPaged(1, 4);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void CustomerController_GetCustomersPaged_ReturnBadRequestResult()
        {
            //Arrange  
            var controller = new CustomersController(customersDAL);
            controller.ModelState.AddModelError("CompanyName", "Required");
            //Act  
            var data = await controller.GetPaged(5, 10);
            //data = null;

            //if (data != null)
            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void CustomerController_GetCustomersPaged_MatchResultCount()
        {
            //Arrange  
            var controller = new CustomersController(customersDAL);

            //Act  
            var data = await controller.GetPaged(1, 4);

            //Assert  
            var okObjectResult = data as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var pagedResult = okObjectResult.Value as PagedResult<Customer>;
            Assert.NotNull(pagedResult);

            Assert.IsType<OkObjectResult>(data);

            Assert.Equal(4, pagedResult.Results.Count);
        }

        #endregion

        #region create
        [Fact]
        public async void CustomerController_CreateCustomer_ReturnOkResult()
        {
            //Arrange  
            var controller = new CustomersController(customersDAL);

            //Act  
            var data = await controller.Post(objTestCustomer);

            // Assert
            var okObjectResult = data as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var model = okObjectResult.Value as Customer;
            Assert.NotNull(model);

            Assert.IsType<OkObjectResult>(okObjectResult);
            //Assert.Equal<HttpStatusCode>(HttpStatusCode.Created, (HttpStatusCode)okObjectResult.StatusCode);
        }

        [Fact]
        public async void CustomerController_CreateCustomer_ReturnBadRequest()
        {
            //Arrange  
            var controller = new CustomersController(customersDAL);
            objTestCustomer.ContactName = "Contact Name is too long - exceeds max character limit";

            var data = await controller.Post(objTestCustomer);
            var actualResult = (StatusCodeResult)data;

            // Revert to valid ContactName
            objTestCustomer.ContactName = testContactName;

            // Assert
            Assert.IsType<BadRequestResult>(data);
            //Assert.Equal<HttpStatusCode>(HttpStatusCode.BadRequest, (HttpStatusCode)actualResult.StatusCode);
        }

        [Fact]
        public async void CustomerController_CreateCustomer_MatchResult()
        {
            //Arrange  
            var controller = new CustomersController(customersDAL);
            
            //Act  
            var data = await controller.Post(objTestCustomer);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var model = okResult.Value.Should().BeAssignableTo<Customer>().Subject;  

            Assert.Equal(objTestCustomer.CompanyName, model.CompanyName);
            Assert.Equal(objTestCustomer.ContactName, model.ContactName);
            Assert.Equal(objTestCustomer.Address, model.Address);
            Assert.Equal(objTestCustomer.City, model.City);
            Assert.Equal(objTestCustomer.Region, model.Region);
            Assert.Equal(objTestCustomer.PostalCode, model.PostalCode);
            Assert.Equal(objTestCustomer.Country, model.Country);
            Assert.Equal(objTestCustomer.Phone, model.Phone);
        }
        #endregion

        #region get by id
        [Fact]
        public async void CustomerController_GetCustomerById_ReturnOkResult()
        {
            //Arrange  
            var controller = new CustomersController(customersDAL);

            // Our test database is seeded with seedRowsCount datarows. Choose a random valid value.
            var rng = new Random();
            var data = await controller.Get(rng.Next(1, seedRowsCount));

            var okObjectResult = data as OkObjectResult;

            Assert.NotNull(okObjectResult);

            var model = okObjectResult.Value;// as Customer;
            Assert.NotNull(model);

            Assert.IsType<Customer>(model);

            //Assert 
            Assert.IsType<OkObjectResult>(data);
            //Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)okObjectResult.StatusCode);
        }

        [Fact]
        public async void CustomerController_GetCustomerById_ReturnNotFoundResult()
        {
            //Arrange  
            var controller = new CustomersController(customersDAL);
            int customerID = -1;

            //Act  
            var data = await controller.Get(customerID);

            //Assert  
            Assert.IsType<NotFoundObjectResult>(data);
        }

        [Fact]
        public async void CustomerController_GetCustomerById_ReturnBadRequestResult()
        {
            //Arrange  
            var controller = new CustomersController(customersDAL);
            int? customerID = null;

            //Act  
            var data = await controller.Get(customerID);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void CustomerController_GetCustomerById_MatchResult()
        {
            //Arrange  
            var controller = new CustomersController(customersDAL);

            //Act  
            // Our test database is seeded with seedRowsCount datarows. Choose a random valid value.
            var rng = new Random();
            var customerIndex = rng.Next(1, seedRowsCount);
            var data = await controller.Get(customerIndex);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var customer = okResult.Value.Should().BeAssignableTo<Customer>().Subject;

            Assert.Equal(SeedData.DummyCompanyNames[customerIndex - 1], customer.CompanyName);
            Assert.Equal(SeedData.DummyContactNames[customerIndex - 1], customer.ContactName);
            Assert.Equal(SeedData.DummyAddresses[customerIndex - 1], customer.Address);
            Assert.Equal(SeedData.DummyCities[customerIndex - 1], customer.City);
            Assert.Equal(SeedData.DummyRegions[customerIndex - 1], customer.Region);
            Assert.Equal(SeedData.DummyPostalCodes[customerIndex - 1], customer.PostalCode);
            Assert.Equal(SeedData.DummyCountries[customerIndex - 1], customer.Country);
            Assert.Equal(SeedData.DummyPhones[customerIndex - 1], customer.Phone);
        }

        #endregion

        #region edit
        [Fact]
        public async void CustomerController_UpdateCustomer_ReturnOkResult()
        {
            //Arrange
            var controller = new CustomersController(customersDAL);

            // Our test database is seeded with seedRowsCount datarows. Choose a random valid value.
            var rng = new Random();
            var customerID = rng.Next(1, seedRowsCount);

            //Act
            var existingPost = await controller.Get(customerID);
            var okResult = existingPost.Should().BeOfType<OkObjectResult>().Subject;
            var result = okResult.Value.Should().BeAssignableTo<Customer>().Subject;

            result.CompanyName += " Updated";
            result.ContactName += " Updated";

            var updatedData = await controller.Put(result);

            //Assert
            Assert.IsType<OkObjectResult>(updatedData);
        }

        [Fact]
        public async void CustomerController_UpdateCustomer_ReturnBadRequest()
        {
            //Arrange
            var controller = new CustomersController(customersDAL);

            // Our test database is seeded with seedRowsCount datarows. Choose a random valid value.
            var rng = new Random();
            var customerID = rng.Next(1, seedRowsCount);

            //Act
            var existingCustomer = await controller.Get(customerID);
            var okResult = existingCustomer.Should().BeOfType<OkObjectResult>().Subject;
            var result = okResult.Value.Should().BeAssignableTo<Customer>().Subject;

            result.CompanyName += " updated to a very long invalid string exceeding max allowed length";
            result.ContactName += " Updated";

            var updatedData = await controller.Put(result);

            //Assert
            Assert.IsType<BadRequestResult>(updatedData);
        }

        [Fact]
        public async void CustomerController_UpdateCustomer_ReturnNotFound()
        {
            //Arrange
            var controller = new CustomersController(customersDAL);

            // Our test database is seeded with seedRowsCount datarows. Choose an valid value.
            var customerBadID = seedRowsCount + 10;
            objTestCustomer.Id = customerBadID;

            //Act
            var data = await controller.Put(objTestCustomer);

            //Assert
            Assert.IsType<NotFoundObjectResult>(data);
        }
        #endregion

        #region delete
        [Fact]
        public async void CustomerController_DeleteCustomer_ReturnOkResult()
        {
            //Arrange  
            var controller = new CustomersController(customersDAL);
           
            // Our test database is seeded with seedRowsCount datarows. Choose a random valid value.
            var rng = new Random();
            var customerID = rng.Next(1, seedRowsCount);

            //Act  
            var data = await controller.Delete(customerID);

            //Assert  
            Assert.IsType<OkResult>(data);
        }

        [Fact]
        public async void CustomerController_DeleteCustomer_ReturnNotFoundResult()
        {
            //Arrange  
            var controller = new CustomersController(customersDAL);

            // Our test database is seeded with seedRowsCount datarows. Choose an valid value.
            var customerBadID = seedRowsCount + 10;

            //Act  
            var data = await controller.Delete(customerBadID);

            //Assert  
            Assert.IsType<NotFoundObjectResult>(data);
        }

        [Fact]
        public async void CustomerController_DeleteCustomer_ReturnBadRequestResult()
        {
            //Arrange  
            var controller = new CustomersController(customersDAL);
            int? postId = null;

            //Act  
            var data = await controller.Delete(postId);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        #endregion
    }
}

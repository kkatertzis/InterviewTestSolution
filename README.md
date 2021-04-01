# Description

Implemented solution of original description (see below).

All required functionalities implemented.

Extra functionalities implemented: UnitTests on API controller & basic IntegrationTests with dummy http client.

Projects included in solution:
- BlazorApp: main project (includes the API implementation as well)
- BlazorAppUnitTest: Unit Test project (xUnit)
- BlazorAppIntegrationTests: Unit Test project (xUnit)
- 
## Installation Notes
BlazorApp and BlazorAppUnitTest projects are configured to run on local Microsoft SQL server database. Projects run on separate databases (CustomerDb and CustomerTestDb).
BlazorAppIntegrationTests project is configured to run with InMemoryDatabase configuration.

Databases and tables are auto-created and seeded with dummy data upon first run.

To use a different database configuration, change settings in
- BlazorApp/StartUp.cs (for main project BlazorApp)
- BlazorAppUnitTest/CustomersUnitTest.cs (for unit test project BlazorAppUnitTest)
- BlazorAppIntegrationTests/CustomWebApplicationFactory.cs (for integration test project BlazorAppIntegrationTests)

If needed, change connection string settings in BlazorApp/appsettings.json and BlazorAppTest/CustomerUnitTest.cs respectively.

Test API by running all unit/integration tests; alternatively, test through Postman or similar tool. API may be accessed at {baseUri}/api/customers. Get paged results at {baseUri}/api/customers/paged?page={pageNumber}&pagesize={pageSize}.


# Description (original)

You are given a solution that contains a Blazor Server Side Web App with a Customer model class. Write the admin panel for the “Customer” model. 

You should fork this project and provide a github link for your solution.

You have to develop 

Required: 
- Configure application to use Entity Framework Core
- A grid with all customers with server side paging
- CRUD Operations on “Customer” model with new, edit and delete functionalities
- Expose all CRUD Operations as an API 

Extra (nice to have) 
- Add authentication with the provided demo Identity Server 4 https://demo.identityserver.io/
- Protect your API with authentication with the provided demo Identity Server above
- Unit & Integration Tests

## Requirements 

- C#
- .NET Core 
- Blazor

Optional
- Bootstrap 

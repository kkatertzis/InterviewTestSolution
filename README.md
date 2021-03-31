# Description

Implemented solution of original description (see below).

All required functionalities implemented.

Extra functionalities implemented: UnitTests on API controller.

Projects included in solution:
- BlazorApp: main project (includes the API implementation as well)
- BlazorAppTest: Unit Test project (xUnit)

## Installation Notes
Both projects are configured to run on local Microsoft SQL server database. To use a different database configuration, change settings in BlazorApp/StartUp.cs and BlazorAppTest/CustomersUnitTest.cs respectively. BlazorAppTest runs on a separate test database (CustomerTestDb).

Databases and tables are auto-created and seeded with dummy data upon first run.
If needed, change connection string settings in BlazorApp/appsettings.json and BlazorAppTest/CustomerUnitTest.cs respectively.

Test API by running all unit tests; alternatively, test through Postman or similar tool.


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

using System.Threading.Tasks;
using BlazorApp.Components;
using Microsoft.AspNetCore.Components;
using BlazorApp.Models;
using System;

namespace BlazorApp.Pages
{
    public partial class EditCustomerModel : CustomerPageBase
    {

        [Parameter]
        public string Id { get; set; } = "0";
        protected string PageTitle { get; private set; }
        protected Customer CurrentCustomer{ get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (Id == null || Id == "0")
            {

                PageTitle = "Add Customer";
                CurrentCustomer = new Customer();
            }
            else
            {
                PageTitle = "Edit Customer";
                await LoadCustomer(int.Parse(Id));
            }
        }

        protected async Task LoadCustomer(int id)
        {
            CurrentCustomer = await CustomersService.GetCustomer(id);
        }

        protected async Task Save()
        {
            if (CurrentCustomer.Id <= 0)
            {
                await CustomersService.CreateCustomer(CurrentCustomer);
            }
            else {
                await CustomersService.UpdateCustomer(CurrentCustomer);
            }

            Cancel();
        }

        protected void Cancel() {
            NavigationManager.NavigateTo("/customers");
        }
        
    }
}

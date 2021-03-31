using System.Threading.Tasks;
using BlazorApp.Components;
using BlazorApp.Models;
using BlazorApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorApp.Pages
{
    public class CustomersModel : CustomerPageBase
    {

        [Parameter]
        public string Page { get; set; } = "1";

        protected int DeleteId { get; set; } = -1;
        protected PagedResult<Customer> Customers;

        protected override async Task OnParametersSetAsync()
        {
            await LoadCustomers(int.Parse(Page ?? "1"));
        }

        private async Task LoadCustomers(int page)
        {
            Customers = await CustomersService.ListPaged(page, 10);
        }

        protected void PagerPageChanged(int page)
        {
            NavigationManager.NavigateTo("/customers/" + page);
        }

        protected void AddNew()
        {
            NavigationManager.NavigateTo("/edit/");
        }

        protected void EditCustomer(int id)
        {
            NavigationManager.NavigateTo("/edit/" + id.ToString());
        }

        protected async void ConfirmDelete(int id, string title)
        {
            DeleteId = id;

            await JSRuntime.InvokeAsync<bool>("customModal.confirmDelete", title);
        }

        protected async Task DeleteCustomer()
        {
            
            await JSRuntime.InvokeAsync<bool>("customModal.hideDeleteDialog");

            await CustomersService.DeleteCustomer(DeleteId);
           
            await LoadCustomers(int.Parse(Page ?? "1"));
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazorApp.Shared;
using Microsoft.AspNetCore.Mvc;
using BlazorApp.Data;

namespace BlazorApp.Components
{
    public abstract class CustomerPageBase : ComponentBase
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected ICustomers CustomersService { get; set; }
    }
}

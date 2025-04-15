using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;

namespace StorageSystem.WEB.Pages.Suppliers
{
    public partial class SuppliersSelect
    {        
        [Inject] private IRepository Repository { get; set; } = null!;        
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        public List<Supplier>? Suppliers { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            var responseHttp = await Repository.GetAsync<List<Supplier>>("api/Suppliers");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error!", message, SweetAlertIcon.Error);
                return;
            }
            Suppliers = responseHttp.Response;
        }
    }
}
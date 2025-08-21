using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;

namespace StorageSystem.WEB.Pages.Products
{
    public partial class ProductsListSelect
    {
        private List<Product>? products { get; set; }
        [Parameter] public EventCallback<string> OnSelectedProductChanged { get; set; }

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await LoadProductsAsync();
        }

        private async Task LoadProductsAsync()
        {
            var responseHttp = await Repository.GetAsync<List<Product>>("/api/Products/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            products = responseHttp.Response;
        }
    }
}
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;

namespace StorageSystem.WEB.Pages.Products
{
    public partial class ProductsRecipesListSelect
    {
        private List<Product>? products { get; set; }
        private List<Recipe>? recipes { get; set; }
        [Parameter] public EventCallback<string> OnSelectedProductRecipeChanged { get; set; }

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await LoadProductsAsync();
            await LoadRecipesAsync();
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

        private async Task LoadRecipesAsync()
        {
            var responseHttp = await Repository.GetAsync<List<Recipe>>("/api/Recipes/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            recipes = responseHttp.Response;
        }
    }
}
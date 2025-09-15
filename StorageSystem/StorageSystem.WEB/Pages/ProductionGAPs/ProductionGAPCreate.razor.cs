using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;

namespace StorageSystem.WEB.Pages.ProductionGAPs
{
    public partial class ProductionGAPCreate
    {
        private ProductionGap productionGap = new();
        private Recipe Recipe = new();

        private int productId;
        private string productName = "Producto";
        private decimal totalBache;
        private decimal cantidadBache;

        private List<Recipe>? Recipes { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;

        protected async override Task OnInitializedAsync()
        {
            await LoadRecipesAsync();            
        }

        private void ClickProductCallBack(string product)
        {
            string[] valores = product.Split(',');
            productId = int.Parse(valores[0]);
            productName = valores[1];
            SelectRecipeId(productId);
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
            Recipes = responseHttp.Response;  
        }

        private async Task CreateAsync()
        {
            try
            {
                if (cantidadBache > 0 && productId != 0)
                {
                    productionGap.Amount = totalBache * cantidadBache;

                    var result = await SweetAlertService.FireAsync(new SweetAlertOptions
                    {
                        Title = "Confirmacion",
                        Text = $"¿Estas seguro de querer crear cantidad de baches: {cantidadBache} del producto: {productName}?",
                        Icon = SweetAlertIcon.Question,
                        ShowCancelButton = true,
                    });

                    var confirm = string.IsNullOrEmpty(result.Value);
                    if (confirm)
                    {
                        return;
                    }
                    var responseHttp = await Repository.PostAsync("/api/ProductionGaps", productionGap);
                    if (responseHttp.Error)
                    {
                        var message = await responseHttp.GetErrorMessageAsync();
                        await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                        return;
                    }
                    NavigationManager.NavigateTo("/productionsgaps");
                    var toast = SweetAlertService.Mixin(new SweetAlertOptions
                    {
                        Toast = true,
                        Position = SweetAlertPosition.BottomEnd,
                        ShowConfirmButton = true,
                        Timer = 3000
                    });
                    await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado con éxito.");                                                          
                }
                else
                {
                    await SweetAlertService.FireAsync("Error", "Todos los campos son obligatorios.", SweetAlertIcon.Error);
                    return;
                }

            }
            catch (Exception ex)
            {
                await SweetAlertService.FireAsync("Error", ex.Message, SweetAlertIcon.Error);
                return;
            }
        }

        private void SelectRecipeId(int productId)
        {
            Recipes = Recipes!.Where(r => r.ProductId == productId).ToList();
            if (Recipes.Count > 0)
            {
                Recipe = Recipes[0];
                productionGap.RecipeId = Recipe.Id;
                totalBache = Recipe.TotalRecipe;
            }
        }
    }
}
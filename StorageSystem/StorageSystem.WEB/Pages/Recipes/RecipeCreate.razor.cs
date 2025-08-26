using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;

namespace StorageSystem.WEB.Pages.Recipes
{
    public partial class RecipeCreate
    {
        private Recipe recipe = new();
        private string productName = "Producto";
        private string rawMaterialName = "Materia prima";
        private string rawMaterialCodeMeasurementUnit = string.Empty;   

        private int rawMaterialId { get; set; }
        private int productId { get; set; }
        private bool isProductButton = false;

        private List<Recipe>? Recipes { get; set; } = null!;
        private List<RawMaterial>? rawMaterials { get; set; } = null!;

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;        

        protected override async Task OnInitializedAsync()
        {
            await LoadRawMaterialsAsync();
        }

        private void ClickProductCallBack(string product)
        {
            string[] valores = product.Split(',');
            productId = int.Parse(valores[0]);
            productName = valores[1];   
            isProductButton = true;
        }

        private void ClickRawMaterialCallBack(string rawMaterial)
        {
            string[] valores = rawMaterial.Split(',');
            rawMaterialId = int.Parse(valores[0]);
            rawMaterialName = valores[1];
            rawMaterialCodeMeasurementUnit = valores[2];
        }        

        private async Task CreateAsync()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(recipe.Amount) && rawMaterialId != 0 && productId != 0)
                {
                    recipe.ProductId = productId;
                    recipe.RawMaterialId = rawMaterialId;
                    var responseHttp = await Repository.PostAsync("/api/Recipes", recipe);
                    if (responseHttp.Error)
                    {
                        var message = await responseHttp.GetErrorMessageAsync();
                        await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                        return;
                    }
                    await LoadRecipesAsync(productId);
                    /*var toast = SweetAlertService.Mixin(new SweetAlertOptions
                    {
                        Toast = true,
                        Position = SweetAlertPosition.BottomEnd,
                        ShowConfirmButton = true,
                        Timer = 3000
                    });                    
                    await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado con éxito.");    */                
                    rawMaterialName = "Materia prima";
                    recipe = new Recipe();
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

        private async Task LoadRecipesAsync(int productId)
        {
            var responseHttp = await Repository.GetAsync<List<Recipe>>("/api/Recipes/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            Recipes = responseHttp.Response;
            Recipes = Recipes!.Where(r => r.ProductId == productId).ToList();
        }

        private async Task LoadRawMaterialsAsync()
        {
            var responseHttp = await Repository.GetAsync<List<RawMaterial>>("/api/RawMaterials/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            rawMaterials = responseHttp.Response;
        }
    }
}
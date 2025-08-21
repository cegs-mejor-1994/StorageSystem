using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;

namespace StorageSystem.WEB.Pages.Recipes
{
    public partial class RecipeCreate
    {
        private Recipe recipe = new();

        private string rawMaterialName = "Materia prima";
        private int rawMaterialId { get; set; }

        private string productName = "Producto";
        private int productId { get; set; }

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;


        private void ClickProductCallBack(string product)
        {
            string[] valores = product.Split(',');
            productId = int.Parse(valores[0]);
            productName = valores[1];
        }

        private void ClickRawMaterialCallBack(string rawMaterial)
        {
            string[] valores = rawMaterial.Split(',');
            rawMaterialId = int.Parse(valores[0]);
            rawMaterialName = valores[1];
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
                    var toast = SweetAlertService.Mixin(new SweetAlertOptions
                    {
                        Toast = true,
                        Position = SweetAlertPosition.BottomEnd,
                        ShowConfirmButton = true,
                        Timer = 3000
                    });
                    await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado con éxito.");
                    productName = "Proveedor";
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
    }
}
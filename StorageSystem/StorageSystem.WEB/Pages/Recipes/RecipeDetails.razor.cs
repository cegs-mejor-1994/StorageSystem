using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Pages.Recipes;
using StorageSystem.WEB.Repositories;

namespace StorageSystem.WEB.Pages.Recipes
{
    public partial class RecipeDetails
    {
        [EditorRequired, Parameter] public int ProductId { get; set; }

        [CascadingParameter] IModalService Modal { get; set; } = default!;

        private string? productName;

        private string rawMaterialName = "Materia prima";
        private string rawMaterialCodeMeasurementUnit = string.Empty;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;

        private List<Recipe>? Recipes { get; set; }

        private Recipe recipe = new();
  
        private int rawMaterialId { get; set; }              
        
        private List<RawMaterial>? rawMaterials { get; set; } = null!;

        protected async override Task OnParametersSetAsync()
        {
            await LoadRecipesAsync(ProductId);
            await LoadRawMaterialsAsync();
        }

        private void ClickRawMaterialCallBack(string rawMaterial)
        {
            string[] valores = rawMaterial.Split(',');
            rawMaterialId = int.Parse(valores[0]);
            rawMaterialName = valores[1];
            rawMaterialCodeMeasurementUnit = "(" + valores[2] + ")";
        }

        private async Task ShowModalAsync(int id)
        {
            IModalReference modalReference;
            modalReference = Modal.Show<RecipeEditById>(string.Empty, new ModalParameters().Add("Id", id));

            var result = await modalReference.Result;
            if (result.Confirmed)
            {
                await LoadRecipesAsync(ProductId);
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

        private string GetProductName()
        {
            if (Recipes != null && Recipes.Count > 0)
            {
                productName = Recipes[0].Product?.Name;
            }
            return productName ?? string.Empty;
        }

        private async Task DeleteAsync(Recipe recipe)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmacion",
                Text = $"¿Estas seguro que quieres borrar la materia prima: {recipe.RawMaterial!.Name} de la formula?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
            });

            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }
            var responseHttp = await Repository.DeleteAsync<Recipe>($"api/Recipes/{recipe.Id}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    var messageError = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", messageError, SweetAlertIcon.Error);
                }
                return;
            }
            await LoadRecipesAsync(ProductId);
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000,
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro borrado con exito");
        }

        private async Task CreateAsync()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(recipe.Amount) && rawMaterialId != 0 && ProductId != 0)
                {
                    recipe.ProductId = ProductId;
                    recipe.RawMaterialId = rawMaterialId;
                    var responseHttp = await Repository.PostAsync("/api/Recipes", recipe);
                    if (responseHttp.Error)
                    {
                        var message = await responseHttp.GetErrorMessageAsync();
                        await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                        return;
                    }
                    await LoadRecipesAsync(ProductId);
                    rawMaterialName = "Materia prima";
                    rawMaterialCodeMeasurementUnit = string.Empty;
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
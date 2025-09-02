using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Pages.Recipes;
using StorageSystem.WEB.Repositories;
using System.Net;

namespace StorageSystem.WEB.Pages.ProductDetails
{
    public partial class ProductDetailsFragments
    {
        [EditorRequired, Parameter] public int ProductId { get; set; }

        [CascadingParameter] IModalService Modal { get; set; } = default!;

        private string referenceName = "Referencia";

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;

        private List<Reference>? References { get; set; }        
        private List<RawMaterial>? rawMaterials { get; set; } = null!;

        private ProductsDetail productDetail = new();
        private Product product = new();        

        private int referenceId { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            var responseHttp = await Repository.GetAsync<Product>($"/api/Products/{ProductId}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync(new SweetAlertOptions { Title = "Error", Text = message, Icon = SweetAlertIcon.Error });
                }
            }
            else
            {
                product = responseHttp.Response!;
            }
            await LoadRecipesAsync(ProductId);
            await LoadRawMaterialsAsync();
            await GetRecipeTotals();
        }

        private void ClickReferenceCallBack(string reference)
        {
            string[] valores = reference.Split(',');
            referenceId = int.Parse(valores[0]);
            referenceName = valores[1];            
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
                if (recipe.Amount == 0 && rawMaterialId != 0 && ProductId != 0)
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

        private async Task EditBatchTotalAsync()
        {
            if (batchTotal == 0)
            {
                recipe2.Id = RecipeTotals!.Where(r => r.RecipeId == ProductId).Select(r => r.Id).FirstOrDefault();
                recipe2.RecipeId = ProductId;
                recipe2.TotalRecipe = (decimal)batchTotal;
                var responseHttp = await Repository.PutAsync($"/api/RecipeTotals", recipe2);
                if (responseHttp.Error)
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message);
                    return;
                }
                await GetRecipeTotals();
                var toast = SweetAlertService.Mixin(new SweetAlertOptions
                {
                    Toast = true,
                    Position = SweetAlertPosition.BottomEnd,
                    ShowConfirmButton = true,
                    Timer = 3000,
                });
                await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Bache Total actualizado con exito");
            }
            else
            {
                await SweetAlertService.FireAsync("Error", "El total de la formula no puede estar vacio.", SweetAlertIcon.Error);
                return;
            }
        }

        private async Task GetRecipeTotals()
        {
            var responseHttp = await Repository.GetAsync<List<RecipeTotal>>("/api/RecipeTotals/full");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            RecipeTotals = responseHttp.Response;
            batchTotal = RecipeTotals!.Where(r => r.RecipeId == ProductId).Select(r => r.TotalRecipe).FirstOrDefault();
        }
    }
}
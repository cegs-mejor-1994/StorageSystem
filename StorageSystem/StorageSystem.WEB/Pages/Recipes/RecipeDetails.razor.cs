using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;
using System.Net;
using static StorageSystem.Shared.Enums.ProductStateAndPhisical;

namespace StorageSystem.WEB.Pages.Recipes
{
    public partial class RecipeDetails
    {
        [EditorRequired, Parameter] public int ProductId { get; set; }

        [CascadingParameter] IModalService Modal { get; set; } = default!;        

        private string productRawMaterialName = "Materia prima";
        private string? unitBaseProduct = "";

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;
        
        private List<RecipeDetail>? RecipeProductDetails { get; set; }        
        private List<Product>? productRecipeRawMaterials { get; set; } = null!;
        private List<Product>? productRawMaterials { get; set; } = null!;        
        private List<MeasurementUnit>? measurementUnits { get; set; }
        private List<MeasurementUnit>? allMeasurementUnits { get; set; }
        private List<Recipe>? Recipe { get; set; }

        private Recipe recipe = new();
        private Product product = new();
        private RecipeDetail recipeDetail = new();        

        private int productRawMaterialId { get; set; }
        private int? measurementUnitId { get; set; }
        private int recipeId { get; set; }
        private double TotalRecipe { get; set; } = 0;

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
            await LoadRawMaterialsAsync();   
            await LoadMeasurementUnitsAsync();
            await GetRecipeByProductId(ProductId);
            await LoadRecipesDetailsAsync(recipeId);
            await GetTotalRecipeInDetails(recipeId);
        }

        private async Task GetRecipeByProductId(int productId)
        {
            var responseHttp = await Repository.GetAsync<List<Recipe>>("/api/Recipes/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            Recipe = responseHttp.Response;
            Recipe = Recipe!.Where(r => r.ProductId == productId).ToList();
            if (Recipe.Count > 0)
            {
                recipeId = Recipe!.Select(r => r.Id).FirstOrDefault();
                await GetTotalRecipeAsync();
            }
        }

        private void ClickRawMaterialCallBack(string productRawMaterial)
        {
            string[] valores = productRawMaterial.Split(',');
            productRawMaterialId = int.Parse(valores[0]);
            productRawMaterialName = valores[1];
            if(product.PhysicalState == ProductPhysicalState.Solido)
            {
                measurementUnits = new List<MeasurementUnit>(allMeasurementUnits!);
                measurementUnits = measurementUnits.Where(mu => mu.PhysicalState.ToString() == "Solido").ToList();
                measurementUnitId = null;
            }
            measurementUnits = new List<MeasurementUnit>(allMeasurementUnits!);
            measurementUnits = measurementUnits.Where(mu => mu.PhysicalState.ToString() == valores[2]).ToList();
            measurementUnitId = null;
        }

        private async Task ShowModalAsync(int id)
        {
            IModalReference modalReference;
            modalReference = Modal.Show<RecipeEditById>(string.Empty, new ModalParameters().Add("Id", id));

            var result = await modalReference.Result;
            if (result.Confirmed)
            {
                await LoadRecipesDetailsAsync(recipeId);
            }
        }

        private async Task LoadRecipesDetailsAsync(int recipeId)
        {
            var responseHttp = await Repository.GetAsync<List<RecipeDetail>>("/api/RecipeDetails/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            RecipeProductDetails = responseHttp.Response;
            RecipeProductDetails = RecipeProductDetails!.Where(r => r.RecipeId == recipeId).ToList();
        }

        private async Task LoadMeasurementUnitsAsync()
        {
            var responseHttp = await Repository.GetAsync<List<MeasurementUnit>>("/api/MeasurementUnits/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            allMeasurementUnits = responseHttp.Response;
            if(allMeasurementUnits != null)
            {
                unitBaseProduct = allMeasurementUnits.Where(mu => mu.Base && mu.PhysicalState == product.PhysicalState).Select(mu => mu.Code).FirstOrDefault();
            }
        }

        private async Task LoadRawMaterialsAsync()
        {
            var responseHttp = await Repository.GetAsync<List<Product>>("/api/Products/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            productRawMaterials = responseHttp.Response;
            if (product.PhysicalState.ToString() == "Solido")
            {
                productRecipeRawMaterials = new List<Product>(productRawMaterials!);
                productRecipeRawMaterials.RemoveAll(p => p.Id == product.Id || p.PhysicalState.ToString() == "Liquido" || (p.Role.ToString() == "ProductoFinal" || p.Role.ToString() == "Presentacion"));                
            }
            else
            {
                productRecipeRawMaterials = new List<Product>(productRawMaterials!);
                productRecipeRawMaterials.RemoveAll(p => p.Id == product.Id || (p.Role.ToString() == "ProductoFinal" || p.Role.ToString() == "Presentacion"));                
            }
        }

        private async Task DeleteAsync(RecipeDetail recipeDetail)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmacion",
                Text = $"¿Estas seguro que quieres borrar la materia prima: {recipeDetail.Product!.Name} de la formula?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
            });

            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }
            var responseHttp = await Repository.DeleteAsync<Recipe>($"api/RecipeDetails/{recipeDetail.Id}");
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
            await GetTotalRecipeInDetails(recipeId);            
            await UpdateTotalRecipeAsync(recipe);
            await LoadRecipesDetailsAsync(recipeId);
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000,
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro borrado con exito");
        }

        private async Task GetTotalRecipeInDetails(int recipeID)
        {
            var url = $"api/RecipeDetails/totalAmount?recipeID={recipeID}";
            var responseHttp = await Repository.GetAsync<double>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            TotalRecipe = responseHttp.Response;
        }

        private async Task GetTotalRecipeAsync()
        {
            var responseHttp = await Repository.GetAsync<Recipe>($"/api/Recipes/{recipeId}");
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
                recipe = responseHttp.Response!;                
            }
        }

        private async Task UpdateTotalRecipeAsync(Recipe recipe)
        {
            recipe.TotalRecipe = (decimal)TotalRecipe;
            var responseHttp = await Repository.PutAsync($"/api/Recipes", recipe);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message);
                return;
            }                     
        }

        private async Task CreateAsync()
        {
            try
            {
                if (recipeDetail.Amount > 0 && productRawMaterialId != 0 && ProductId != 0 && measurementUnitId != 0)
                {
                    recipeDetail.RecipeId = recipeId; 
                    recipeDetail.ProductId = productRawMaterialId;
                    recipeDetail.MeasurementUnitId = measurementUnitId!.Value;                    
                    var responseHttp = await Repository.PostAsync("/api/RecipeDetails", recipeDetail);
                    if (responseHttp.Error)
                    {
                        var message = await responseHttp.GetErrorMessageAsync();
                        await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                        return;
                    }
                    await GetTotalRecipeInDetails(recipeId);
                    await UpdateTotalRecipeAsync(recipe);
                    await LoadRecipesDetailsAsync(recipeId);
                    productRawMaterialName = "Materia prima";                    
                    recipeDetail = new RecipeDetail();
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
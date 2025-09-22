using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;
using System.Net;

namespace StorageSystem.WEB.Pages.ProductionGAPs
{
    public partial class ProductionGAPCreate
    {
        private ProductionGap productionGap = new();
        private Recipe Recipe = new();         
        private InputInventory? inputInventory;

        private int productId;
        private string productName = "Producto";
        private string productNamesNotExists = "";
        private string productsWithLowAmount = "";
        private decimal totalBache;
        private decimal cantidadBache = 1;
        private bool GAPValidated = true;

        private List<Recipe>? RecipesMain { get; set; }
        private List<RecipeDetail>? RecipeDetails { get; set; }
        private List<Recipe>? Recipes { get; set; }
        
        private List<InputInventory>? InputInventories { get; set; }  

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;

        protected async override Task OnInitializedAsync()
        {                       
            await LoadInputInventoriesAsync();
        }

        private async Task ClickProductCallBack(string product)
        {
            string[] valores = product.Split(',');
            productId = int.Parse(valores[0]);
            productName = valores[1];
            await LoadRecipesAsync();
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
            RecipesMain = responseHttp.Response;
            if (RecipesMain != null)
            {
                if (RecipesMain.Count > 0)
                {
                    Recipes = new List<Recipe>(RecipesMain);
                    Recipes = Recipes.Where(r => r.ProductId == productId).ToList();
                    Recipe = Recipes[0];
                    productionGap.RecipeId = Recipe.Id;
                    totalBache = Recipe.TotalRecipe;
                    await LoadRecipeDetailsAsync(Recipe.Id);
                }
            }
        }

        private async Task LoadInputInventoriesAsync()
        {
            var responseHttp = await Repository.GetAsync<List<InputInventory>>("/api/InputInventories/InputInventoriesCombo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            InputInventories = responseHttp.Response;
            InputInventories = InputInventories?.Where(p => p.Product!.Role == StorageSystem.Shared.Enums.ProductStateAndPhisical.ProductRole.MateriaPrima).ToList();
        }

        private async Task LoadRecipeDetailsAsync(int recipeID)
        {
            var url = $"/api/RecipeDetails/FactorConversionDetails?recipeID={recipeID}";
            var responseHttp = await Repository.GetAsync<List<RecipeDetail>>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            RecipeDetails = responseHttp.Response;
        }

        private void CompareInputInventoryWithRecipeDetails(decimal cantidadBache)
        {
            if (RecipeDetails!.Count == 0)
            {
                SweetAlertService.FireAsync("Error", "No hay receta creada.", SweetAlertIcon.Error);
                GAPValidated = false;
                return;
            }

            foreach (var RecipeDetail in RecipeDetails!)
            {
                var primerInventario = InputInventories!.Where(ii => ii.LeftAmount > 0 && ii.ProductId == RecipeDetail.ProductId).OrderBy(ii => ii.RegisterDate).FirstOrDefault();

                if (primerInventario == null)
                {
                    productNamesNotExists += $"{RecipeDetail.Product!.Name},";
                    GAPValidated = false;                    
                }
                else
                {
                    var amountNeccesaryForBatch = (double)RecipeDetail.Amount * (double)cantidadBache;
                    if ((double)primerInventario.LeftAmount <= amountNeccesaryForBatch)
                    {
                        productsWithLowAmount += $"{RecipeDetail.Product!.Name}, Cantidad: {primerInventario.LeftAmount}  {primerInventario.MeasurementUnit!.Code}, se necesita: {amountNeccesaryForBatch} {RecipeDetail.MeasurementUnit!.Code};";
                        GAPValidated = false;                        
                    }
                }
            }
        }

        private async Task CreateAsync()
        {
            CompareInputInventoryWithRecipeDetails(cantidadBache);
            if (GAPValidated && cantidadBache > 0 && productId != 0)
            {
                await SweetAlertService.FireAsync("Info", "Se puede crear el bache", SweetAlertIcon.Info);
                /*var result = await SweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Confirmacion",
                    Text = $"¿Estas seguro de querer crear cantidad de baches: {cantidadBache} del producto: {productName}?",
                    Icon = SweetAlertIcon.Question,
                    ShowCancelButton = true,
                });*/

                /*var confirm = string.IsNullOrEmpty(result.Value);
                if (confirm)
                {
                    return;
                }
                
                foreach (var RecipeDetail in RecipeDetails!)
                {
                    var primerInventario = InputInventories!.Where(ii => ii.LeftAmount > 0 && RecipeDetails.Any(rd => rd.ProductId == ii.ProductId)).OrderBy(ii => ii.RegisterDate).FirstOrDefault();

                    if (primerInventario != null)
                    {
                        await GetInputInventoryById(primerInventario.Id);
                        if(inputInventory != null)
                        {
                            var amountRecipeDetail = ConvertToUnitBaseFromRecipeDetail(RecipeDetail);
                            var amountInputInventory = ConvertToUnitBaseFromInputInventory(inputInventory);
                            var amountLeftAfterCreateBatch = amountInputInventory - amountRecipeDetail;
                            inputInventory.LeftAmount = (decimal)amountLeftAfterCreateBatch;
                            await UpdateLeftAmount(inputInventory);
                        }                      
                    }
                }  */

                /* try {                    
                     productionGap.Amount = totalBache * cantidadBache;

                     var responseHttp = await Repository.PostAsync("/api/ProductionGaps", productionGap);
                     if (responseHttp.Error)
                     {
                         var message = await responseHttp.GetErrorMessageAsync();
                         await SweetAlertService.FireAsync("Error", "Error al guardar bache", SweetAlertIcon.Error);
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
                 catch (Exception ex)
                 {
                     await SweetAlertService.FireAsync("Error", ex.Message, SweetAlertIcon.Error);
                     return;
                 }*/
            }
            else
            {
                if (RecipeDetails!.Count == 0)
                {
                    await SweetAlertService.FireAsync("Error", $"No se pudo crear el bache. No hay receta creada", SweetAlertIcon.Error);
                    return;
                }
                var mensajes = new List<string>();

                if (!string.IsNullOrEmpty(productNamesNotExists))
                {
                    mensajes.Add($"Materias primas sin inventario: {productNamesNotExists.TrimEnd(',')}.");
                }

                if (!string.IsNullOrEmpty(productsWithLowAmount))
                {
                    mensajes.Add($"Materias primas con cantidades insuficientes: {productsWithLowAmount.TrimEnd(';')}.");
                }

                if (mensajes.Count > 0)
                {
                    await SweetAlertService.FireAsync(
                        "Error",
                        $"No se pudo crear el bache.\n\n{string.Join("\n\n", mensajes)}",
                        SweetAlertIcon.Error
                    );
                }
            }       
        }

        private async Task UpdateLeftAmount(InputInventory inputRDInventory)
        {
            if(inputRDInventory != null)
            {
                var responseHttp = await Repository.PutAsync($"/api/InputInventories", inputRDInventory);
                if (responseHttp.Error)
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", "Error al actualizar cantidad de bache");
                    return;
                }
            }
        }
    }
}
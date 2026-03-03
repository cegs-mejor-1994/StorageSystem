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
        private ProductionGap productionGapAfterCreated = new();
        private Recipe Recipe = new();         
        private ProductionGapDetail ProductionGapDetail = new();
        private InputInventory? InputInventory;

        private int productId;
        private int countProductionGap = 0;
        
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
        private List<ProductionGap>? ProductionGaps { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;

        protected async override Task OnInitializedAsync()
        {                       
            await LoadInputInventoriesAsync();
            await LoadProductionGapsAsync();
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
                    countProductionGap = ProductionGaps!.Where(pg => pg.RecipeId == Recipe.Id).Count();
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

        private async Task LoadProductionGapsAsync()
        {
            var responseHttp = await Repository.GetAsync<List<ProductionGap>>("/api/ProductionGaps/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            ProductionGaps = responseHttp.Response;           
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
                /*var primerInventario = InputInventories!.Where(ii => ii.LeftAmount > 0 && ii.ProductId == RecipeDetail.ProductId).OrderBy(ii => ii.RegisterDate).FirstOrDefault();

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
                        productsWithLowAmount += $"{RecipeDetail.Product!.Name}, Cantidad: {primerInventario.LeftAmount}  {primerInventario.Product!.MeasurementUnit!.Code}, se necesita: {amountNeccesaryForBatch} {RecipeDetail.MeasurementUnit!.Code};";
                        GAPValidated = false;                        
                    }
                }*/

                var amountNeccesaryForBatch = RecipeDetail.Amount * cantidadBache;
                

                // Traer TODOS los lotes del producto una sola vez
                var inventariosProducto = InputInventories!
                    .Where(ii => ii.LeftAmount > 0 && ii.ProductId == RecipeDetail.ProductId)
                    .OrderBy(ii => ii.RegisterDate) // FIFO
                    .ToList();

                if (!inventariosProducto.Any())
                {
                    productNamesNotExists += $"{RecipeDetail.Product!.Name},";
                    GAPValidated = false;
                }
                else
                {
                    // Buscar el primer lote que cumpla con la cantidad necesaria
                    var inventarioValido = inventariosProducto
                        .FirstOrDefault(ii => ii.LeftAmount >= amountNeccesaryForBatch);

                    if (inventarioValido == null)
                    {
                        // Obtener el lote con mayor cantidad solo para mostrar información
                        var mayorLote = inventariosProducto
                            .OrderByDescending(ii => ii.LeftAmount)
                            .First();

                        productsWithLowAmount += $"{RecipeDetail.Product!.Name}, " +
                            $"Mayor lote disponible: {mayorLote.LeftAmount} {mayorLote.Product!.MeasurementUnit!.Code}, " +
                            $"se necesita: {amountNeccesaryForBatch} {RecipeDetail.MeasurementUnit!.Code};";

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

                try {            
                    var countGap = countProductionGap + 1;
                    productionGap.Amount =  totalBache * cantidadBache;
                    productionGap.ControlCode = countGap.ToString();
                    productionGap.LeftAmount = totalBache * cantidadBache;

                    var responseHttp = await Repository.PostAsync<ProductionGap, ProductionGap>("/api/ProductionGaps", productionGap);                    
                    if (responseHttp.Error)
                    {
                        var message = await responseHttp.GetErrorMessageAsync();
                        await SweetAlertService.FireAsync("Error", "Error al guardar bache", SweetAlertIcon.Error);
                        return;
                    }

                    productionGapAfterCreated = responseHttp.Response!;

                    
                    var toast = SweetAlertService.Mixin(new SweetAlertOptions
                    {
                        Toast = true,
                        Position = SweetAlertPosition.BottomEnd,
                        ShowConfirmButton = true,
                        Timer = 3000
                    });
                    await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Bache creado con exito.");

                    foreach (var RecipeDetail in RecipeDetails!)
                    {

                        var amountNeccesaryForBatch = RecipeDetail.Amount * cantidadBache;

                        var inventariosProducto = InputInventories!.Where(ii => ii.LeftAmount > 0 && ii.ProductId == RecipeDetail.ProductId).OrderBy(ii => ii.RegisterDate).ToList(); 

                        var primerInventario = inventariosProducto.FirstOrDefault(ii => ii.LeftAmount >= amountNeccesaryForBatch);

                        if (primerInventario != null)
                        {
                            await GetInputInventoryById(primerInventario.Id);
                            var amountLeftAfterCreateBatch = primerInventario.LeftAmount - amountNeccesaryForBatch;
                            if (InputInventory != null)
                            {
                                InputInventory.LeftAmount = amountLeftAfterCreateBatch;
                                await UpdateLeftAmount(InputInventory);
                                if (productionGapAfterCreated != null)
                                {
                                    ProductionGapDetail.ProductionGapId = productionGapAfterCreated.Id;
                                    ProductionGapDetail.InputInventoryId = InputInventory.Id;
                                    ProductionGapDetail.Amount = RecipeDetail.Amount * cantidadBache;
                                    await CreateProductionGAPDetail(ProductionGapDetail);
                                }
                            }
                        }
                    }
                    NavigationManager.NavigateTo("/productionsgaps");
                }
                catch (Exception ex)
                {
                    await SweetAlertService.FireAsync("Error", ex.Message, SweetAlertIcon.Error);
                    return;
                }                
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
                    if (cantidadBache == 1)
                    {
                        await SweetAlertService.FireAsync(
                            "Error",
                            $"No se pudo crear un bache de {productName}.\n\n{string.Join("\n\n", mensajes)}",
                            SweetAlertIcon.Error
                        );
                    }
                    else {
                        await SweetAlertService.FireAsync(
                            "Error",
                            $"No se pudo crear {cantidadBache} baches de {productName}.\n\n{string.Join("\n\n", mensajes)}",
                            SweetAlertIcon.Error
                        );
                    }
                }
                productNamesNotExists = "";
                productsWithLowAmount = "";
                cantidadBache = 1;                
            }       
        }

        private async Task CreateProductionGAPDetail(ProductionGapDetail productionGapDetail)
        {
            var responseHttp = await Repository.PostAsync("/api/ProductionGAPDetails", productionGapDetail);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error","Error al crear detalle de bache", SweetAlertIcon.Error);
                return;
            }
        }

        private async Task GetInputInventoryById(int id)
        {
            var responseHttp = await Repository.GetAsync<InputInventory>($"/api/InputInventories/{id}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync(new SweetAlertOptions { Title = "Error", Text = message, Icon = SweetAlertIcon.Error });
                }
                else
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync(new SweetAlertOptions { Title = "Error", Text = "Error al buscar dato de inventario", Icon = SweetAlertIcon.Error });
                }
            }
            else
            {
                InputInventory = responseHttp.Response;
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
                    await SweetAlertService.FireAsync("Error", "Error al actualizar inventario", SweetAlertIcon.Error);
                    return;
                }
            }
        }
    }
}
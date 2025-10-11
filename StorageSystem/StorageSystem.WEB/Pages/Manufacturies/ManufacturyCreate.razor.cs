using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.DTOs;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;
using System.Net;
using System.Text.RegularExpressions;
using static StorageSystem.Shared.Enums.ProductStateAndPhisical;

namespace StorageSystem.WEB.Pages.Manufacturies
{
    public partial class ManufacturyCreate
    {
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;

        private List<ProductDetailDTO>? ProductDetailDTOs { get; set; }
        private List<ProductsDetailStructure>? ProductDetailStructures { get; set; }
        private List<InputInventory>? inputInventories { get; set; }
        private List<MeasurementUnit>? measurementUnits { get; set; }

        private Manufactury Manufactury { get; set; } = new Manufactury();
        private ManufacturyDTO? manufacturyDTO { get; set; }
        private InputInventory? InputInventory;
        private ProductionGap? ProductionGAP;

        private int productDetailId { get; set; }
        private int productId { get; set; }
        private int productDetailReferenceValue { get; set; }
        private int productDetailMeasurementUnitId { get; set; }

        private string productDetailName = "Producto";
        private string productNamesNotExists = "";
        private string productsWithLowAmount = "";
        private string PhysicalState = "";
        private decimal amount = 1;

        private double TotalRecipe { get; set; }
        private double TotalBatchCalculated { get; set; }  
        private double factorConversion { get; set; } = 0;

        private bool InputInventoryValidated = true;        

        protected override async Task OnInitializedAsync()
        {
            await LoadProductDetailsAsync();
            await LoadInputInventoriesAsync();  
            await LoadMeasurementUnitsAsync();
        }

        private string GetMeasurementUnitName(string physicalState)
        {
            if (measurementUnits != null)
            {
                var measurementUnit = measurementUnits.Where(mu => mu.PhysicalState.ToString() == physicalState && mu.Base).FirstOrDefault();
                if (measurementUnit != null)
                {
                    return measurementUnit.Code;
                }
            }

            return "";
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
            inputInventories = responseHttp.Response;
            inputInventories = inputInventories?.Where(p => p.Product!.Role == ProductRole.Presentacion).ToList();
        }

        private async Task LoadProductDetailsAsync()
        {
            var responseHttp = await Repository.GetAsync<List<ProductDetailDTO>>("/api/ProductsDetails/comboReferences");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            ProductDetailDTOs = responseHttp.Response;            
        }

        private string ProductNameSetted(string productName)
        {
            return Regex.Replace(productName, @"\s*\d+.*$", "");
        }

        private async Task ClickProductCallBack(string product)
        {
            string[] valores = product.Split(',');
            productDetailId = int.Parse(valores[0]);
            productId = int.Parse(valores[1]);
            productDetailName = valores[2];
            productDetailReferenceValue = int.Parse(valores[3]);
            productDetailMeasurementUnitId = int.Parse(valores[4]);
            PhysicalState = valores[5];
            await LoadProductDetailStructuresAsync(productDetailId);
            await GetTotalRecipeInProductDetails(productId);
        }

        private async Task CalculateTotalBatch(string physycalState, int meausementUnitFactorId)
        {
            var url = $"api/MeasurementUnits/baseUnitWithFactor?physycalState={physycalState}&meausementUnitFactorId={meausementUnitFactorId}";
            var responseHttp = await Repository.GetAsync<double>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            factorConversion = responseHttp.Response;
            if (factorConversion > 0)
            {
                TotalBatchCalculated = (double)amount * factorConversion * (double)productDetailReferenceValue;
            }
        }

        private async Task GetTotalRecipeInProductDetails(int productID)
        {
            var url = $"api/ProductsDetails/ManufacturyDetail?ProductID={productID}";
            var responseHttp = await Repository.GetAsync<ManufacturyDTO>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            manufacturyDTO = responseHttp.Response;
            if (manufacturyDTO != null)
            {
                await GetProductionGap(manufacturyDTO!.ProductionGapID);
                await GetTotalRecipeInDetails(manufacturyDTO!.RecipeID);
            }            
        }

        private async Task LoadProductDetailStructuresAsync(int productDetailId)
        {
            var url = $"api/ProductsDetailStructures/combo?productDetailID={productDetailId}";
            var responseHttp = await Repository.GetAsync<List<ProductsDetailStructure>>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            ProductDetailStructures = responseHttp.Response;

            foreach (var ProductDetailStructureDTO in ProductDetailStructures!)
            {
                var primerInventario = inputInventories!.Where(ii => ii.LeftAmount > amount && ii.ProductId == ProductDetailStructureDTO.ProductId).OrderBy(ii => ii.RegisterDate).FirstOrDefault();

                if (primerInventario == null)
                {
                    productNamesNotExists += $"{ProductDetailStructureDTO.Product!.Name}, ";
                    InputInventoryValidated = false;
                }
                else
                {                  
                    if (primerInventario.LeftAmount <= amount)
                    {
                        productsWithLowAmount += $"{ProductDetailStructureDTO.Product!.Name}, Cantidad: {primerInventario.LeftAmount}, se necesita: {amount};";
                        InputInventoryValidated = false;
                    }
                }
            }
        }

        private async Task CreateAsync()
        {
            if (InputInventoryValidated && productDetailId > 0 && manufacturyDTO != null)
            {
                await CalculateTotalBatch(PhysicalState, productDetailMeasurementUnitId);
                if (TotalRecipe > TotalBatchCalculated)
                {                    
                    Manufactury.ProductsDetailId = productDetailId;
                    Manufactury.ProductionGapId = ProductionGAP!.Id;  
                    Manufactury.Amount = amount;

                    var responseHttp = await Repository.PostAsync("/api/Manufacturies", Manufactury);
                    if (responseHttp.Error)
                    {
                        var message = await responseHttp.GetErrorMessageAsync();
                        await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                        return;
                    }
                    var newLeftAmout = TotalRecipe - TotalBatchCalculated;
                    
                    if (ProductionGAP != null)
                    {
                        ProductionGAP.LeftAmount = (decimal)newLeftAmout;
                        await EditLeftProductGAPAsync(ProductionGAP);
                    }

                    foreach (var ProductDetailStructure in ProductDetailStructures!)
                    {
                        var primerInventario = inputInventories!.Where(ii => ii.LeftAmount > amount && ii.ProductId == ProductDetailStructure.ProductId).OrderBy(ii => ii.RegisterDate).FirstOrDefault();

                        if (primerInventario != null)
                        {
                            await GetInputInventoryById(primerInventario.Id);
                            var amountLeftAfterCreateBatch = primerInventario.LeftAmount - amount;
                            if (InputInventory != null)
                            {
                                InputInventory.LeftAmount = amountLeftAfterCreateBatch;
                                await UpdateLeftAmount(InputInventory);
                            }
                        }
                    }

                    NavigationManager.NavigateTo("/manufacturies");
                    var toast = SweetAlertService.Mixin(new SweetAlertOptions
                    {
                        Toast = true,
                        Position = SweetAlertPosition.BottomEnd,
                        ShowConfirmButton = true,
                        Timer = 3000
                    });
                    await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Producto terminado creado con éxito.");
                }
                else
                {
                    await SweetAlertService.FireAsync("Info", $"No se pudo fabricar el producto. Cantidad disponible de {ProductNameSetted(productDetailName)}: {TotalRecipe} {GetMeasurementUnitName(PhysicalState)}. Cantidad necesaria: {TotalBatchCalculated} {GetMeasurementUnitName(PhysicalState)}", SweetAlertIcon.Info);
                }
            }
            else
            {
                if (ProductDetailStructures!.Count == 0)
                {
                    await SweetAlertService.FireAsync("Error", $"No se ha creado la presentacion para el producto {productDetailName}", SweetAlertIcon.Error);
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
                        $"No se pudo crear el producto terminado.\n\n{string.Join("\n\n", mensajes)}",
                        SweetAlertIcon.Error
                    );
                }
                productNamesNotExists = "";
                productsWithLowAmount = "";
                amount = 1;
                InputInventoryValidated = true;
                productDetailName = "Producto";
                productDetailId = 0;
                productId = 0;                
                productDetailReferenceValue = 0;
                productDetailMeasurementUnitId = 0;
                PhysicalState = "";
                factorConversion = 0;
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
            if (inputRDInventory != null)
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

        private async Task GetProductionGap(int productionGapId)
        {
            var responseHttp = await Repository.GetAsync<ProductionGap>($"/api/ProductionGaps/{productionGapId}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/categories");
                }
                else
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync(new SweetAlertOptions { Title = "Error", Text = message, Icon = SweetAlertIcon.Error });
                }
            }
            else
            {
                ProductionGAP = responseHttp.Response;
            }
        }

        private async Task EditLeftProductGAPAsync(ProductionGap productionGap)
        {
            var responseHttp = await Repository.PutAsync($"/api/ProductionGaps", productionGap);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message);
                return;
            }            
        }

        private async Task GetTotalRecipeInDetails(int recipeID)
        {
            var url = $"api/ProductionGaps/totalBatch?recipeId={recipeID}";
            var responseHttp = await Repository.GetAsync<double>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            TotalRecipe = responseHttp.Response;
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
            measurementUnits = responseHttp.Response;
        }
    }
}
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;

namespace StorageSystem.WEB.Pages.InputInventories
{
    public partial class InputInventoryCreate
    {        
        private InputInventory inputInventory = new();  
        
        private string productName = "Materia prima";
        private string productRole = "";

        private int productlId { get; set; }       
        private int? measurementUnitId { get; set; }
        private int CountInputInventories { get; set; }

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        private List<MeasurementUnit>? allMeasurementUnits { get; set; }
        private List<MeasurementUnit>? measurementUnits { get; set; }
        private List<InputInventory>? inputInventories { get; set; }

        protected override async Task OnInitializedAsync()
        {                      
            await LoadMeasurementUnitsAsync();
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
            CountInputInventories = inputInventories!.Count + 1;
        }

        private void ClickProductCallBack(string product)
        {            
            string[] valores = product.Split(',');
            productlId = int.Parse(valores[0]);
            productName = valores[1];
            productRole = valores[3];
            if (productRole == "Presentacion")
            {
                measurementUnits = new List<MeasurementUnit>(allMeasurementUnits!);
                measurementUnits = measurementUnits.Where(mu => mu.Id == 5).ToList();
                measurementUnitId = null;
                inputInventory.Batch = "N/A";
                inputInventory.MatutingDate = DateTime.Now;
            }
            else
            {
                measurementUnits = new List<MeasurementUnit>(allMeasurementUnits!);
                measurementUnits = measurementUnits.Where(mu => mu.PhysicalState.ToString() == valores[2] && mu.Base).ToList();
                measurementUnitId = null;
            }
        }

        private async Task CreateAsync()
        {
            try
            {
                if (inputInventory.Amount > 0 && !string.IsNullOrWhiteSpace(inputInventory.Batch) && inputInventory.MatutingDate != DateTime.MinValue && productlId != 0 && measurementUnitId != 0)
                {
                    await LoadInputInventoriesAsync();
                    inputInventory.ProductId = productlId;
                    inputInventory.ControlCode = CountInputInventories.ToString();
                    inputInventory.MeasurementUnitId = measurementUnitId!.Value;
                    inputInventory.LeftAmount = inputInventory.Amount;

                    var responseHttp = await Repository.PostAsync("/api/InputInventories", inputInventory);
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
                    NavigationManager.NavigateTo("/inputinventories");
                    await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado con éxito.");                    
                } else {                     
                    await SweetAlertService.FireAsync("Error", "Todos los campos son obligatorios ", SweetAlertIcon.Error);
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
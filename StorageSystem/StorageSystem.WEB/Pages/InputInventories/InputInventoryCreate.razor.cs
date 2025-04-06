using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;

namespace StorageSystem.WEB.Pages.InputInventories
{
    public partial class InputInventoryCreate
    {
        private List<InputInventory> inputInventories = new();
        private InputInventory inputInventory = new();
        private List<Supplier>? suppliers;
        private List<RawMaterial>? rawMaterials;

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await LoadSuppliersAsync();
            await LoadRawMaterialsAsync();
        }

        private async Task CreateAsync()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(inputInventory.Amount) && !string.IsNullOrWhiteSpace(inputInventory.Batch) && inputInventory.MatutingDate != DateTime.MinValue && inputInventory.RawMaterialId != 0 && inputInventory.SupplierId != 0)
                {
                    AddAsync(inputInventory);
                }
                
                if (inputInventories.Count > 0)
                {
                    foreach (var inputInventory in inputInventories)
                    {
                        //measurementUnit.DateRegister = DateTime.Now;
                        var responseHttp = await Repository.PostAsync("/api/inputInventories", inputInventory);
                        if (responseHttp.Error)
                        {
                            var message = await responseHttp.GetErrorMessageAsync();
                            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                            return;
                        }
                    }
                    var toast = SweetAlertService.Mixin(new SweetAlertOptions
                    {
                        Toast = true,
                        Position = SweetAlertPosition.BottomEnd,
                        ShowConfirmButton = true,
                        Timer = 3000
                    });
                    await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado con éxito.");
                    NavigationManager.NavigateTo("/inputInventories");
                }
            }
            catch (Exception ex)
            {
                await SweetAlertService.FireAsync("Error", ex.Message, SweetAlertIcon.Error);
                return;
            }                       
        }

        private async void AddAsync(InputInventory input)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(inputInventory.Amount) || string.IsNullOrWhiteSpace(inputInventory.Batch) || inputInventory.MatutingDate == DateTime.MinValue || inputInventory.RawMaterialId == 0 || inputInventory.SupplierId == 0)
                {
                    throw new Exception("Debes llenar el formulario para guardar");
                }
                inputInventories.Add(input);
                inputInventory = new();
                //GetInputInventories();                  
            }
            catch (Exception ex)
            {
                await SweetAlertService.FireAsync("Error", ex.Message, SweetAlertIcon.Error);
            }           
        }

        private void DeleteAsync(InputInventory input)
        {
            inputInventories.Remove(input);
            //GetInputInventories();            
        }

        private string GetRawMaterialName(int rawMaterialId)
        {
            var rawMaterial = rawMaterials?.FirstOrDefault(r => r.Id == rawMaterialId);
            return rawMaterial?.Name!;
        }

        private string GetSupplierName(int supplierId)
        {
            var supplier = suppliers?.FirstOrDefault(s => s.Id == supplierId);
            return supplier?.Name!;
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

        private async Task LoadSuppliersAsync()
        {
            var responseHttp = await Repository.GetAsync<List<Supplier>>("/api/Suppliers/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            suppliers = responseHttp.Response;
        }

       /* private List<InputInventory> GetInputInventories()
        {
            return inputInventories;
        }*/        
    }
}
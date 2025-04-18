using Blazored.Modal;
using Blazored.Modal.Services;
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
        private string rawMaterialName { get; set;  } = null!;
        private string supplierName { get; set; } = null!;

        private int rawMaterialId { get; set; } 
        private int supplierId { get; set; }

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await LoadSuppliersAsync();
            await LoadRawMaterialsAsync();
            supplierId = 0;
            rawMaterialId = 0;
        }        

        private async Task CreateAsync()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(inputInventory.Amount) && !string.IsNullOrWhiteSpace(inputInventory.Batch) && inputInventory.MatutingDate != DateTime.MinValue && rawMaterialId != 0 && supplierId != 0)
                {
                    inputInventory.SupplierId = supplierId;
                    inputInventory.RawMaterialId = rawMaterialId;
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
                if (string.IsNullOrWhiteSpace(inputInventory.Amount) || string.IsNullOrWhiteSpace(inputInventory.Batch) || inputInventory.MatutingDate == DateTime.MinValue || rawMaterialId == 0 || supplierId == 0)
                {
                    throw new Exception("Debes llenar el formulario para guardar");
                }
                input.SupplierId = supplierId;
                input.RawMaterialId = rawMaterialId;
                inputInventories.Add(input);
                inputInventory = new();
                Clear();
            }
            catch (Exception ex)
            {
                await SweetAlertService.FireAsync("Error", ex.Message, SweetAlertIcon.Error);
            }           
        }

        private void DeleteAsync(InputInventory input)
        {
            inputInventories.Remove(input);                       
        }

        private string GetRawMaterialName(int id)
        {
            var rawMaterial = rawMaterials?.FirstOrDefault(r => r.Id == id);
            rawMaterialName = rawMaterial?.Name!;
            rawMaterialId = id;
            return rawMaterial?.Name!;
        }

        private string GetSupplierName(int id)
        {
            var supplier = suppliers?.FirstOrDefault(s => s.Id == id);
            supplierId = id;
            supplierName = supplier?.Name!;
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
       
        private void Clear()
        {
            supplierId = default;
            rawMaterialId = default;
        }
    }
}
using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Pages.RawMaterials;
using StorageSystem.WEB.Pages.Suppliers;
using StorageSystem.WEB.Repositories;

namespace StorageSystem.WEB.Pages.InputInventories
{
    public partial class InputInventoryCreate
    {
        [CascadingParameter] IModalService Modal { get; set; } = default!;

        private List<InputInventory> inputInventories = new();
        private InputInventory inputInventory = new();
        private List<Supplier>? suppliers;
        private List<RawMaterial>? rawMaterials;
        private string rawMaterialName { get; set;  } = null!;
        private string supplierName { get; set; } = null!;

        [Parameter] public int rawMaterialId { get; set; }
        [Parameter] public int supplierId { get; set; }

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await LoadSuppliersAsync();
            await LoadRawMaterialsAsync();            
        }

        private void ShowRawMaterialListModalAsync()
        {
            IModalReference modalReference = Modal.Show<RawMaterialsSelect>();
        }

        private void ShowSupplierListModalAsync()
        {
            IModalReference modalReference = Modal.Show<SuppliersSelect>();
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
        }

        private string GetRawMaterialName(int rawMaterialId)
        {
            var rawMaterial = rawMaterials?.FirstOrDefault(r => r.Id == rawMaterialId);
            rawMaterialName = rawMaterial?.Name!;
            return rawMaterial?.Name!;
        }

        private string GetSupplierName(int supplierId)
        {
            var supplier = suppliers?.FirstOrDefault(s => s.Id == supplierId);
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
        
        private void HandleRawMaterialChanged(ChangeEventArgs e)
        {           
            string nuevoValor = GetRawMaterialName(Convert.ToInt32(e.Value?.ToString()));
            rawMaterialName = nuevoValor;            
        }

        private void HandleSupplierChanged(ChangeEventArgs e)
        {
            string nuevoValor = GetSupplierName(Convert.ToInt32(e.Value?.ToString()));
            supplierName = nuevoValor;
        }
    }
}
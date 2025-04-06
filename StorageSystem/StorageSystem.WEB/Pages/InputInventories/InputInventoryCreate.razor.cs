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
            if (inputInventory.Amount != null && inputInventory.Batch != null && inputInventory.MatutingDate != null && inputInventory.RawMaterialId != 0 && inputInventory.SupplierId != 0)
            {
                if (inputInventories.Count == 0)
                {
                    var responseHttp = await Repository.PostAsync("/api/inputInventories", inputInventory);
                    if (responseHttp.Error)
                    {
                        var message = await responseHttp.GetErrorMessageAsync();

                        if (message!.Contains("requerido"))
                        {
                            await SweetAlertService.FireAsync("Error", "Debes llenar el formulario para guardar", SweetAlertIcon.Error);
                            return;
                        }

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
                    await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado con éxito.");
                    NavigationManager.NavigateTo("/inputInventories");
                }
                else
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
            else
            {
                await SweetAlertService.FireAsync("Error", "Debes digitar por lo menos un registro para Guardar Cambios", SweetAlertIcon.Error);
                return;
            }
        }

        private async void AddAsync(InputInventory input)
        {
            if (inputInventory.Amount != null && inputInventory.Batch != null && inputInventory.MatutingDate != null && inputInventory.RawMaterialId != 0 && inputInventory.SupplierId != 0)
            {
                if (inputInventories.Count <= 4)
                {
                    inputInventories.Add(input);
                    inputInventory = new();
                    GetInputInventories();
                }
                else
                {
                    await SweetAlertService.FireAsync("Informacion", $"Guarda cambios con los {inputInventories.Count} registros y haz otro proceso.", SweetAlertIcon.Info);
                }
            }
            else
            {
                await SweetAlertService.FireAsync("Error", "Debes digitar por lo menos un registro para agregar", SweetAlertIcon.Error);
            }
        }

        private void DeleteAsync(InputInventory input)
        {
            inputInventories.Remove(input);
            GetInputInventories();            
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

        private List<InputInventory> GetInputInventories()
        {
            return inputInventories;
        }
        
    }
}
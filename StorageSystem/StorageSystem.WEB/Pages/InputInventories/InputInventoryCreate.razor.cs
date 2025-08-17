using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;
using System.Net;

namespace StorageSystem.WEB.Pages.InputInventories
{
    public partial class InputInventoryCreate
    {
        private List<InputInventory> inputInventories = new();
        private InputInventory inputInventory = new();       
        private string supplierName = "Proveedor";
        private string rawMaterialName = "Materia prima";        

        private int rawMaterialId { get; set; } 
        private int supplierId { get; set; }

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;


        void ClickSupplierCallBack (string supplier)
        {            
            string[] valores = supplier.Split(',');
            supplierId = int.Parse(valores[0]);
            supplierName = valores[1];             
        }

        void ClickRawMaterialCallBack(string rawMaterial)
        {            
            string[] valores = rawMaterial.Split(',');
            rawMaterialId = int.Parse(valores[0]);
            rawMaterialName = valores[1];              
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
                rawMaterialName = "Materia prima";
                supplierName = "Proveedor";
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
    }
}
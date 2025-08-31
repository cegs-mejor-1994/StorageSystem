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
        private InputInventory inputInventory = new();       
        private string supplierName = "Proveedor";
        private string rawMaterialName = "Materia prima";        

        private int rawMaterialId { get; set; } 
        private int supplierId { get; set; }

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        private void ClickSupplierCallBack (string supplier)
        {            
            string[] valores = supplier.Split(',');
            supplierId = int.Parse(valores[0]);
            supplierName = valores[1];             
        }

        private void ClickRawMaterialCallBack(string rawMaterial)
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
                    await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado con éxito.");
                    supplierName = "Proveedor";
                    rawMaterialName = "Materia prima";
                    inputInventory = new InputInventory();
                } else {                     
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
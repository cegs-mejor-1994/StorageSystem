using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;

namespace StorageSystem.WEB.Pages.Suppliers
{
    public partial class SupplierCreate
    {
        private Supplier supplier = new();
        private Supplier? supplierEdited;
        private SupplierForm? supplierForm;

        [Inject] private IRepository repository { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;

        private async Task CreateAsync()
        {                        
            var responseHttp = await repository.GetAsync<int>($"/api/Suppliers/GetSupplierById?SNit={supplier.Nit}");
            switch (responseHttp.Response)
            {
                case 0:
                    await AddSupplier(supplier);
                    break;

                case -1:
                    await sweetAlertService.FireAsync("Error", "Ya existe un registro activo con ese codigo y con ese nombre", SweetAlertIcon.Error);
                    break;

                case > 0:
                    var result = await sweetAlertService.FireAsync(new SweetAlertOptions
                    {
                        Title = "¿Desea reactivar el registro?",
                        Text = "Ya existe un registro eliminado con ese nit. ¿Desea reactivarlo?",
                        Icon = SweetAlertIcon.Question,
                        ShowCancelButton = true,
                        ConfirmButtonText = "Sí, reactivar",
                        CancelButtonText = "No, cancelar"
                    });
                    if (result.IsConfirmed)
                    {
                        supplierEdited = new Supplier
                        {
                            Id = responseHttp.Response,
                            Nit = supplier.Nit,
                            Name = supplier.Name,
                            Address = supplier.Address,
                            Phone = supplier.Phone,
                            City = supplier.City, 
                            State = "Disponible"
                        };
                        await UpdateSupplier(supplierEdited);
                    }
                    break;

                default:
                    var message = await responseHttp.GetErrorMessageAsync();
                    await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                    break;
            }
        }

        private void Return()
        {
            supplierForm!.FormPostedSuccessfully = true;
            navigationManager.NavigateTo("/suppliers");
        }

        private async Task Message(string message)
        {            
            var toast = sweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: message);
        }

        private async Task AddSupplier(Supplier supplier)
        {
            var responseHttp = await repository.PostAsync("/api/Suppliers", supplier);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            Return();
            await Message("Registro creado con éxito.");
        }

        private async Task UpdateSupplier(Supplier supplier)
        {
            var responseHttpPut = await repository.PutAsync($"/api/Suppliers/", supplier);
            if (responseHttpPut.Error)
            {
                var messagePut = await responseHttpPut.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", messagePut, SweetAlertIcon.Error);
                return;
            }
            Return();
            await Message("Registro reactivado con éxito.");
        }
    }
}
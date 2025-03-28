using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;
using StorageSystem.WEB.Shared;

namespace StorageSystem.WEB.Pages.RawMaterials
{
    public partial class RawMaterialCreate
    {
        private RawMaterial rawMaterial = new();
        private FormWithFields<RawMaterial>? rawMaterialForm;

        [Inject] private IRepository repository { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;

        private async Task CreateAsync()
        {
            //measurementUnit.DateRegister = DateTime.Now;
            var responseHttp = await repository.PostAsync("/api/rawMaterials", rawMaterial);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            Return();
            var toast = sweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado con éxito.");
        }
        private void Return()
        {
            rawMaterialForm!.FormPostedSuccessfully = true;
            navigationManager.NavigateTo("/rawMaterials");
        }
    }
}
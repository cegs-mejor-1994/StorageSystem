using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;

namespace StorageSystem.WEB.Pages.References
{
    public partial class ReferenceCreate
    {
        private Reference reference = new();
        private int measurementUnitId { get; set; }
        private string? measurementUnitName { get; set; }

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        void ClickMeasurementUnitCallBack(string measurementUnit)
        {
            string[] valores = measurementUnit.Split(',');
            measurementUnitId = int.Parse(valores[0]);
            measurementUnitName = valores[1];
        }

        private async Task CreateAsync()
        {
            reference.MeasurementUnitId = measurementUnitId;            
            var responseHttp = await Repository.PostAsync("/api/References", reference);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            NavigationManager.NavigateTo("/references");
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado con éxito.");
        }
    }
}
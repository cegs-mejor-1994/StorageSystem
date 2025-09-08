using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Pages.MeasurementUnits;
using StorageSystem.WEB.Repositories;

namespace StorageSystem.WEB.Pages.MeasurementsConversion
{
    public partial class MeasurementConversionCreate
    {
        private List<MeasurementUnit>? measurementUnits { get; set; }

        private MeasurementConversion measurementConversion = new MeasurementConversion();

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;        

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
            measurementUnits = responseHttp.Response;
        }

        private async Task CreateAsync()
        {
            //measurementUnit.DateRegister = DateTime.Now;
            var responseHttp = await Repository.PostAsync("/api/MeasurementConversions", measurementConversion);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            
            NavigationManager.NavigateTo("/measurementConversions");

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
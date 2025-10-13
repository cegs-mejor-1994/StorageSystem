using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
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
            if (measurementConversion.FromUnitId == 0) {
                await SweetAlertService.FireAsync("Error", "Debes seleccionar una Unidad de medida origen", SweetAlertIcon.Error);
                return;
            }

            if (measurementConversion.ToUnitId == 0) {
                await SweetAlertService.FireAsync("Error", "Debes seleccionar una Unidad de medida destino", SweetAlertIcon.Error);
                return;
            }

            if (measurementConversion.Factor == 0) {
                await SweetAlertService.FireAsync("Error", "Debes digitar un factor de conversion", SweetAlertIcon.Error);
                return;
            }
            
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
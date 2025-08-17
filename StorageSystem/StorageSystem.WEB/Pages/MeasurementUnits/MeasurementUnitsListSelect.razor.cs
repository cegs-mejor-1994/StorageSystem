using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;

namespace StorageSystem.WEB.Pages.MeasurementUnits
{
    public partial class MeasurementUnitsListSelect
    {
        private List<MeasurementUnit>? measurementUnits { get; set; }
        [Parameter] public EventCallback<string> OnSelectedMeasurementUnitChanged { get; set; }

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

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
    }
}
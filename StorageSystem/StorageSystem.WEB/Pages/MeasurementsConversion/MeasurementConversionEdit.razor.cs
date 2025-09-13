using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;
using StorageSystem.WEB.Shared;
using System.Net;

namespace StorageSystem.WEB.Pages.MeasurementsConversion
{
    public partial class MeasurementConversionEdit
    {
        private MeasurementConversion? measurementConversion; 
        private List<MeasurementUnit>? measurementUnits;

        private string? fromUnitCode;
        private string? toUnitCode;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;

        [EditorRequired, Parameter] public int Id { get; set; }

        protected async override Task OnParametersSetAsync()
        {            
            var responseHttp = await Repository.GetAsync<MeasurementConversion>($"/api/MeasurementConversions/{Id}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync(new SweetAlertOptions { Title = "Error", Text = message, Icon = SweetAlertIcon.Error });
                }
            }
            else
            {
                measurementConversion = responseHttp.Response;
                await LoadMeasurementConversionsAsync();
                fromUnitCode = GetMeasurementCodeById(measurementConversion!.FromUnitId);
                toUnitCode = GetMeasurementCodeById(measurementConversion!.ToUnitId);
            }
        }

        private async Task LoadMeasurementConversionsAsync()
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

        private string GetMeasurementCodeById(int id)
        {
            var measurementUnit = measurementUnits?.FirstOrDefault(mu => mu.Id == id);
            if (measurementUnit != null)
            {
                return measurementUnit.Code;
            }
            return string.Empty;
        }

        private async Task EditAsync()
        {
            var responseHttp = await Repository.PutAsync($"/api/MeasurementConversions", measurementConversion);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message);
                return;
            }
          
            NavigationManager.NavigateTo("/measurementsConversions");

            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000,
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Cambios guardados con exito");
        }
    }
}
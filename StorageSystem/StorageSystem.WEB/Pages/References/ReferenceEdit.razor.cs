using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;
using StorageSystem.WEB.Shared;
using System.Net;

namespace StorageSystem.WEB.Pages.References
{
    public partial class ReferenceEdit
    {
        private Reference? reference;
        private int measurementUnitId { get; set; }
        private string? measurementUnitName { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;

        [EditorRequired, Parameter] public int Id { get; set; }

        void ClickMeasurementUnitCallBack(string measurementUnit)
        {
            string[] valores = measurementUnit.Split(',');
            measurementUnitId = int.Parse(valores[0]);
            measurementUnitName = valores[1];
        }

        protected async override Task OnParametersSetAsync()
        {
            var responseHttp = await Repository.GetAsync<Reference>($"/api/References/{Id}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/references");
                }
                else
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync(new SweetAlertOptions { Title = "Error", Text = message, Icon = SweetAlertIcon.Error });
                }
            }
            else
            {
                reference = responseHttp.Response;
            }
        }

        private async Task EditAsync()
        {
            var responseHttp = await Repository.PutAsync($"/api/References", reference);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message);
                return;
            }
            NavigationManager.NavigateTo("/references");

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
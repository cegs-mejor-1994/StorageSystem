using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;
using System;
using System.Net;
using System.Xml.Linq;
using static StorageSystem.Shared.Enums.ProductStateAndPhisical;

namespace StorageSystem.WEB.Pages.MeasurementUnits
{
    public partial class MeasurementUnitCreate
    {
        private MeasurementUnit measurementUnit = new();
        private MeasurementUnit? measurementUnitEdited;
        
        [Inject] private IRepository repository { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;        

        public List<ProductPhysicalState> PhysicalStats { get; set; } = Enum.GetValues(typeof(ProductPhysicalState)).Cast<ProductPhysicalState>().ToList();

        private async Task CreateAsync()
        {
            var responseHttp = await repository.GetAsync<int>($"/api/MeasurementUnits/getMeasurementById?MCode={measurementUnit.Code}&MName={measurementUnit.Name}");
            
            switch (responseHttp.Response)
            {
                case 0:
                    await AddMeasurementUnit(measurementUnit);                                        
                    break;

                case -1:
                    await sweetAlertService.FireAsync("Error", "Ya existe un registro activo con ese codigo y con ese nombre", SweetAlertIcon.Error);
                    break;
                
                case > 0:
                    var result = await sweetAlertService.FireAsync(new SweetAlertOptions
                    {
                        Title = "¿Desea reactivar el registro?",
                        Text = "Ya existe un registro eliminado con ese código o nombre. ¿Desea reactivarlo?",
                        Icon = SweetAlertIcon.Question,
                        ShowCancelButton = true,
                        ConfirmButtonText = "Sí, reactivar",
                        CancelButtonText = "No, cancelar"
                    });
                    if (result.IsConfirmed)
                    {
                        measurementUnitEdited = new MeasurementUnit
                        {
                            Id = responseHttp.Response,
                            Code = measurementUnit.Code,
                            Name = measurementUnit.Name,
                            PhysicalState = measurementUnit.PhysicalState,
                            State = "Disponible"
                        };
                        await UpdateStateOfMeasurementUnit(measurementUnitEdited);                        
                    }
                    break;

                    default:
                        var message = await responseHttp.GetErrorMessageAsync();
                        await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                        break;
            }            
        }

        private async Task AddMeasurementUnit(MeasurementUnit measurementUnit)
        {
            var responseHttp = await repository.PostAsync("/api/MeasurementUnits", measurementUnit);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }            
            await Return("Registro creado con éxito.");
        }

        private async Task UpdateStateOfMeasurementUnit(MeasurementUnit measurementUnit)
        {
            var responseHttpPut = await repository.PutAsync($"/api/MeasurementUnits/", measurementUnit);
            if (responseHttpPut.Error)
            {
                var messagePut = await responseHttpPut.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", messagePut, SweetAlertIcon.Error);
                return;
            }            
            await Return("Registro reactivado con éxito.");
        }

        private async Task Return(string Message)
        {
            navigationManager.NavigateTo("/measurementUnits");
            var toast = sweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: Message);
        }
    }
}
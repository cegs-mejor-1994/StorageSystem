using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;
using static StorageSystem.Shared.Enums.ProductStateAndPhisical;

namespace StorageSystem.WEB.Pages.MeasurementUnits
{
    public partial class MeasurementUnitCreate
    {
        private MeasurementUnit measurementUnit = new();        
        
        [Inject] private IRepository repository { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;

        public List<ProductPhysicalState> PhysicalStats { get; set; } = Enum.GetValues(typeof(ProductPhysicalState)).Cast<ProductPhysicalState>().ToList();

        private async Task CreateAsync()
        {            
            var responseHttp = await repository.PostAsync("/api/MeasurementUnits", measurementUnit);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            
            navigationManager.NavigateTo("/measurementUnits");

            var toast = sweetAlertService.Mixin(new SweetAlertOptions
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
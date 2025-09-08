using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;
using System.Net;

namespace StorageSystem.WEB.Pages.RawMaterials
{
    public partial class RawMaterialEdit
    {
        private RawMaterial? rawMaterial;
        private List<Category>? categories;
        private List<MeasurementUnit>? measurementUnits;
        private string categoryName { get; set; } = null!;
        private string measurementUnitName { get; set; } = null!;
        
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;

        [EditorRequired, Parameter] public int Id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadCategoriesAsync();
            await LoadMeasurementUnitsAsync();
        }

        protected async override Task OnParametersSetAsync()
        {
            var responseHttp = await Repository.GetAsync<RawMaterial>($"/api/RawMaterials/{Id}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/rawMaterials");
                }
                else
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync(new SweetAlertOptions { Title = "Error", Text = message, Icon = SweetAlertIcon.Error });
                }
            }
            else
            {
                rawMaterial = responseHttp.Response;
                /*categoryName = GetCategoryName(rawMaterial!.CategoryId);
                measurementUnitName = GetMeasurementUnitName(rawMaterial!.MeasurementUnitId);*/
            }
        }

        private async Task EditAsync()
        {
            var responseHttp = await Repository.PutAsync($"/api/RawMaterials", rawMaterial);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message);
                return;
            }            
            NavigationManager.NavigateTo("/rawMaterials");
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000,
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Cambios guardados con exito");
        }

        private async Task LoadCategoriesAsync()
        {
            var responseHttp = await Repository.GetAsync<List<Category>>("/api/Categories/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            categories = responseHttp.Response;
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

        private string GetCategoryName(int id)
        {
            var category = categories!.FirstOrDefault(x => x.Id == id);
            return category!.Name;
        }

        private string GetMeasurementUnitName(int id)
        {
            var measurementUnit = measurementUnits!.FirstOrDefault(x => x.Id == id);
            return measurementUnit!.Name;
        }
    }
}
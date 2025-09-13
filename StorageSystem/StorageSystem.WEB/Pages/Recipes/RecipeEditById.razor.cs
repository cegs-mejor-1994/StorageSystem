using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;
using System.Net;

namespace StorageSystem.WEB.Pages.Recipes
{
    public partial class RecipeEditById
    {
        private RecipeDetail? RecipeDetail;
        private Product? Product;
        private MeasurementUnit? MeasurementUnit;

        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;

        [EditorRequired, Parameter] public int Id { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;

        private List<MeasurementUnit>? measurementUnits { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await GetMeasurementUnits();
        }

        protected async override Task OnParametersSetAsync()
        {
            var responseHttp = await Repository.GetAsync<RecipeDetail>($"/api/RecipeDetails/{Id}");
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
                RecipeDetail = responseHttp.Response;
            }

            if (RecipeDetail != null)
            {               
                await GetProduct(RecipeDetail.ProductId);   
                GetMeasurementUnitById(RecipeDetail.MeasurementUnitId);
            }
        }

        private async Task GetProduct(int id)
        {
            var responseHttp = await Repository.GetAsync<Product>($"/api/Products/{id}");
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
                Product = responseHttp.Response;
            }
        }

        private async Task GetMeasurementUnits()
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

       private void GetMeasurementUnitById(int id)
        {
            if (measurementUnits != null) {
                MeasurementUnit = measurementUnits.FirstOrDefault(x => x.Id == id);
            }
        }

        private async Task EditAsync()
        {
            RecipeDetail!.ProductId = Product!.Id;
            RecipeDetail!.MeasurementUnitId = MeasurementUnit!.Id;
            var responseHttp = await Repository.PutAsync($"/api/RecipeDetails", RecipeDetail);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message);
                return;
            }

            await BlazoredModal.CloseAsync(ModalResult.Ok());            
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
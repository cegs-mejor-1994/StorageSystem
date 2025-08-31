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
        private Recipe? Recipe;
        //private Product? Product { get; set; }
        private List<RawMaterial>? RawMaterials { get; set; }
        private RawMaterial? RawMaterial { get; set; }

        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;

        [EditorRequired, Parameter] public int Id { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;

        protected async override Task OnInitializedAsync()
        {
            await GetRawMaterials();
        }

        protected async override Task OnParametersSetAsync()
        {
            var responseHttp = await Repository.GetAsync<Recipe>($"/api/Recipes/{Id}");
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
                Recipe = responseHttp.Response;
            }

            if (Recipe != null)
            {               
                //await GetProduct(Recipe.ProductId);                
                GetRawMaterial(Recipe.RawMaterialId);
            }
        }

       /* private async Task GetProduct(int id)
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
        }*/

        private async Task GetRawMaterials()
        {
            var responseHttp = await Repository.GetAsync<List<RawMaterial>>("/api/RawMaterials/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            RawMaterials = responseHttp.Response;
        }

        private void GetRawMaterial(int id)
        {
            if (RawMaterials != null) {                
                RawMaterial = RawMaterials.FirstOrDefault(x => x.Id == id);
            }
        }

        private async Task EditAsync()
        {
            //Recipe!.ProductId = Product!.Id;
            //Recipe.RawMaterialId = RawMaterial!.Id;
            var responseHttp = await Repository.PutAsync($"/api/Recipes", Recipe);
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
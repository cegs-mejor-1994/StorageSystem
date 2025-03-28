using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;
using StorageSystem.WEB.Shared;
using System.Net;

namespace StorageSystem.WEB.Pages.Categories
{
    public partial class CategoryEdit
    {
        private Category? category;
        private FormWithFields<Category>? categoryForm;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;

        [EditorRequired, Parameter] public int Id { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            var responseHttp = await Repository.GetAsync<Category>($"/api/categories/{Id}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/categories");
                }
                else
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync(new SweetAlertOptions { Title = "Error", Text = message, Icon = SweetAlertIcon.Error });
                }
            }
            else
            {
                category = responseHttp.Response;
            }
        }

        private async Task EditAsync()
        {
            var responseHttp = await Repository.PutAsync($"/api/categories", category);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message);
                return;
            }
            Return();
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000,
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Cambios guardados con exito");
        }

        private void Return()
        {
            categoryForm!.FormPostedSuccessfully = true;
            NavigationManager.NavigateTo("/categories");
        }
    }
}
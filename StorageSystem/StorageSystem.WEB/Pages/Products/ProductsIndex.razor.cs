using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Pages.Suppliers;
using StorageSystem.WEB.Repositories;
using System.Net;

namespace StorageSystem.WEB.Pages.Products
{
    public partial class ProductsIndex
    {
        [CascadingParameter] IModalService Modal { get; set; } = default!;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        public List<Product>? Products { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            var responseHttp = await Repository.GetAsync<List<Product>>("api/Products");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error!", message, SweetAlertIcon.Error);
                return;
            }
            Products = responseHttp.Response;
        }

        private async Task ShowModalAsync(int id = 0, bool isEdit = false)
        {
            IModalReference modalReference;
            if (isEdit)
            {
                modalReference = Modal.Show<ProductEdit>(string.Empty, new ModalParameters().Add("Id", id));
            }
            else
            {
                modalReference = Modal.Show<ProductCreate>();
            }

            var result = await modalReference.Result;
            if (result.Confirmed)
            {
                await LoadAsync();
            }
        }

        private async Task DeleteAsync(Product product)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmacion",
                Text = $"¿Estas seguro de querer eliminar el producto: {product.Name}?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
            });

            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }
            var responseHttp = await Repository.DeleteAsync<Product>($"api/Products/{product.Id}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/products");
                }
                else
                {
                    var messageError = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", messageError, SweetAlertIcon.Error);
                }
                return;
            }
            await LoadAsync();

            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000,
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro eliminado correctamente");
        }
    }
}
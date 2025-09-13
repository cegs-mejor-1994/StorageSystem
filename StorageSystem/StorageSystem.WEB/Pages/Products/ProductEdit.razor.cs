using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;
using System.Net;
using static StorageSystem.Shared.Enums.ProductStateAndPhisical;

namespace StorageSystem.WEB.Pages.Products
{
    public partial class ProductEdit
    {
        private Product? product;   
        private Supplier? supplier; 
        private RawMaterial? rawMaterial;
        private Category? category;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;

        public List<ProductRole> Roles { get; set; } = Enum.GetValues(typeof(ProductRole)).Cast<ProductRole>().Where(r => r != ProductRole.MateriaPrima).ToList();

        [EditorRequired, Parameter] public int Id { get; set; }

        private string roleString = "";
        private string physicalStateString = "";

        protected async override Task OnParametersSetAsync()
        {
            if (product is not null)
            {
                roleString = product.Role.ToString();
                physicalStateString = product.PhysicalState.ToString();
            }

            var responseHttp = await Repository.GetAsync<Product>($"/api/Products/{Id}");
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
                product = responseHttp.Response;
                if(product!.CategoryId != null)
                {
                    await GetCategory(product.CategoryId.Value);
                }
            }

            if (product != null && product.Role == ProductRole.MateriaPrima)
            {
                await GetRawMaterial(product.Id);                
            }
        }

        private async Task GetSupplier(int id)
        {
            var responseHttp = await Repository.GetAsync<Supplier>($"/api/Suppliers/{id}");
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
                supplier = responseHttp.Response;
            }
        }

        private async Task GetRawMaterial(int id)
        {
            var responseHttp = await Repository.GetAsync<RawMaterial>($"/api/RawMaterials/{id}");
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
                rawMaterial = responseHttp.Response;
                await GetSupplier(rawMaterial!.SupplierId);
            }
        }

        private async Task GetCategory(int id)
        {
            var responseHttp = await Repository.GetAsync<Category>($"/api/Categories/{id}");
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
                category = responseHttp.Response;
            }
        }

        private async Task EditAsync()
        {
            var responseHttp = await Repository.PutAsync($"/api/Products", product);
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
            NavigationManager.NavigateTo("/products");
        }
    }
}
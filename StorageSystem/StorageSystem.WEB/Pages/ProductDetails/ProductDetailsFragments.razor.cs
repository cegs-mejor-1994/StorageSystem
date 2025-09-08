using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;
using System.Net;
using System.Threading.Tasks;

namespace StorageSystem.WEB.Pages.ProductDetails
{
    public partial class ProductDetailsFragments
    {
        [EditorRequired, Parameter] public int ProductId { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;

        private List<ProductsDetail>? ProductsDetails { get; set; }
        private List<RawMaterial>? rawMaterials { get; set; } = null!;        

        private ProductsDetail productsDetail = new();
        private Product product = new();

        private bool referenceSelected = false;
        private int rawMaterialId { get; set; }    
        private int appearanceReferenceId { get; set; }
        private string appearanceReferenceName = "Referencia";

        protected async override Task OnParametersSetAsync()
        {
            var responseHttp = await Repository.GetAsync<Product>($"/api/Products/{ProductId}");
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
                product = responseHttp.Response!;
            }
            await LoadProductDetailsAsync(ProductId);
            await LoadRawMaterialsAsync();     
            //await LoadAppearanceReferencesAsync();
        }

        private async Task ClickRawMaterialCallBack(string rawMaterial)
        {
            string[] valores = rawMaterial.Split(',');
            rawMaterialId = int.Parse(valores[0]);
            //await CreateAsync();
        }

        private void ClickAppearanceReferenceCallBack(string appearanceReference)
        {
            string[] valores = appearanceReference.Split(',');
            appearanceReferenceId = int.Parse(valores[0]);
            appearanceReferenceName = valores[1] + " " + valores[2];
            referenceSelected = true;
        }

        private async Task LoadRawMaterialsAsync()
        {
            var responseHttp = await Repository.GetAsync<List<RawMaterial>>("/api/RawMaterials/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            rawMaterials = responseHttp.Response;
        }

        /*private async Task LoadAppearanceReferencesAsync()
        {
            var responseHttp = await Repository.GetAsync<List<AppearanceReference>>("/api/AppearanceReferences/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            appearanceReferences = responseHttp.Response;
        }*/

        private async Task LoadProductDetailsAsync(int productId)
        {
            var responseHttp = await Repository.GetAsync<List<ProductsDetail>>("/api/ProductsDetails/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            ProductsDetails = responseHttp.Response;
            ProductsDetails = ProductsDetails!.Where(r => r.ProductId == productId).ToList();
        }

        /*private async Task DeleteAsync(ProductsDetail productsDetail)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmacion",
                Text = $"¿Estas seguro que quieres borrar la materia prima de presentacion {productsDetail.RawMaterial!.Name} del producto {product.Name}?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
            });

            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }
            var responseHttp = await Repository.DeleteAsync<ProductsDetail>($"api/ProductsDetails/{productsDetail.Id}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    var messageError = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", messageError, SweetAlertIcon.Error);
                }
                return;
            }
            await LoadProductDetailsAsync(ProductId);
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000,
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro borrado con exito");
        }*/

       /* private async Task CreateAsync()
        {
            try
            {
                if (rawMaterialId != 0 && ProductId != 0)
                {
                    productsDetail.AppearanceReferenceId = appearanceReferenceId;
                    productsDetail.ProductId = ProductId;
                    productsDetail.RawMaterialId = rawMaterialId;
                    var responseHttp = await Repository.PostAsync("/api/ProductsDetails", productsDetail);
                    if (responseHttp.Error)
                    {
                        var message = await responseHttp.GetErrorMessageAsync();
                        await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                        return;
                    }
                    await LoadProductDetailsAsync(ProductId);
                    appearanceReferenceName = "Referencia";
                    productsDetail = new ProductsDetail();
                }
                else
                {
                    await SweetAlertService.FireAsync("Error", "Todos los campos son obligatorios.", SweetAlertIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                await SweetAlertService.FireAsync("Error", ex.Message, SweetAlertIcon.Error);
                return;
            }
        }*/
    }
}

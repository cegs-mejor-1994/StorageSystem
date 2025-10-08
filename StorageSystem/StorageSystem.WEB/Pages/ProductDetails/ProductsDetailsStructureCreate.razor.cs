using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Pages.Recipes;
using StorageSystem.WEB.Repositories;
using System.Net;
using static StorageSystem.Shared.Enums.ProductStateAndPhisical;

namespace StorageSystem.WEB.Pages.ProductDetails
{
    public partial class ProductsDetailsStructureCreate
    {
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        [EditorRequired, Parameter] public int Id { get; set; }

        private ProductsDetail? productsDetail;
        private ProductsDetailStructure productsDetailStructure = new();

        private List<ProductsDetailStructure>? productsDetailStructures = new();
        private List<Product>? productRawMaterials = new();
        private List<Reference>? references = new();
        private List<Product>? products = new();
        private List<string> categoriasFiltro = new List<string>();
        private string productName = "Producto";
        private string categoryProductName = "";
        private string productNameSelected = "";        
        private string referenceNameSelected = "";

        private int productlId { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await LoadProductsAsync();
            await LoadReferencesAsync();
        }

        protected async override Task OnParametersSetAsync()
        {
            var responseHttp = await Repository.GetAsync<ProductsDetail>($"/api/ProductsDetails/{Id}");
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
                productsDetail = responseHttp.Response!;                                
                if (productsDetail != null)
                {
                    await LoadProductDetailsAsync(productsDetail.Id);
                    GetProductById(productsDetail.ProductId);
                    GetReferenceById(productsDetail.ReferenceId);
                    await LoadRawMaterialsAsync();
                }                                
            }
        }
         
        private async Task LoadRawMaterialsAsync()
        {
            var responseHttp = await Repository.GetAsync<List<Product>>("/api/Products/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            productRawMaterials = responseHttp.Response;
            productRawMaterials = productRawMaterials!.Where(p => p.Role == ProductRole.Presentacion && !categoriasFiltro.Contains(p.Category!.Name)).ToList();
        }

        private async Task LoadProductsAsync()
        {
            var responseHttp = await Repository.GetAsync<List<Product>>("/api/Products/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            products = responseHttp.Response;
        }

        private void GetProductById(int productId)
        {
            var product = products!.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {                
                productNameSelected = product.Name;
            }
        }

        private void GetReferenceById(int referenceId)
        {
            var reference = references!.FirstOrDefault(r => r.Id == referenceId);
            if (reference != null)
            {
                referenceNameSelected = reference.MeasurementUnit!.Name;
            }
        }   

        private async Task LoadReferencesAsync()
        {
            var responseHttp = await Repository.GetAsync<List<Reference>>("/api/References/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            references = responseHttp.Response;
        }

        private async Task LoadProductDetailsAsync(int producDetailId)
        {
            var responseHttp = await Repository.GetAsync<List<ProductsDetailStructure>>($"/api/ProductsDetailStructures/combo?productDetailID={producDetailId}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            productsDetailStructures = responseHttp.Response;
            productsDetailStructures = productsDetailStructures!.Where(r => r.ProductsDetailId == producDetailId).ToList();
            if(productsDetailStructures != null)
            {
                foreach (var item in productsDetailStructures)
                {
                    if(item.Product != null && item.Product.Category != null)
                    {
                        if (!categoriasFiltro.Contains(item.Product.Category.Name))
                        {
                            categoriasFiltro.Add(item.Product.Category.Name);
                        }
                    }                    
                }
            }
        }

        private void ClickProductCallBack(string product)
        {
            string[] valores = product.Split(',');
            productlId = int.Parse(valores[0]);
            productName = valores[1];
            categoryProductName = valores[2];
        }

        private async Task CreateAsync()
        {
            try
            {
                if (productlId != 0)
                {
                    productsDetailStructure.ProductsDetailId = productsDetail!.Id;
                    productsDetailStructure.ProductId = productlId;
                    var responseHttp = await Repository.PostAsync("/api/ProductsDetailStructures", productsDetailStructure);
                    if (responseHttp.Error)
                    {
                        var message = await responseHttp.GetErrorMessageAsync();
                        await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                        return;
                    }
                    categoriasFiltro.Add(categoryProductName);
                    await LoadProductDetailsAsync(productsDetail.Id);
                    await LoadRawMaterialsAsync();
                    productsDetailStructure = new();
                    productName = "Producto";
                }
                else
                {
                    await SweetAlertService.FireAsync("Error", "Debes seleccionar una materia prima.", SweetAlertIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                await SweetAlertService.FireAsync("Error", ex.Message, SweetAlertIcon.Error);
                return;
            }
        }

        private async Task DeleteAsync(ProductsDetailStructure productsDetailStructure)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmacion",
                Text = $"¿Estas seguro que quieres borrar la materia prima: {productsDetailStructure.Product!.Name} de la presentacion?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
            });

            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }
            var responseHttp = await Repository.DeleteAsync<ProductsDetailStructure>($"api/ProductsDetailStructures/{productsDetailStructure.Id}");
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
            await LoadProductDetailsAsync(productsDetail!.Id);
            categoriasFiltro.Remove(productsDetailStructure.Product!.Category!.Name);
            await LoadRawMaterialsAsync();
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000,
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro borrado con exito");
        }
    }
}
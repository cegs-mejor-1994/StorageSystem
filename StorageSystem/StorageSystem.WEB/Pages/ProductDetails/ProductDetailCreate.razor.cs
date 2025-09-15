using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;
using System.Threading.Tasks;

namespace StorageSystem.WEB.Pages.ProductDetails
{
    public partial class ProductDetailCreate
    {
        private ProductsDetail productsDetail = new();

        private string productName = "Producto";
        private string productPhysicalState = string.Empty;        

        private int productlId { get; set; }        

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        
        private List<int>? ReferencesIds { get; set; }
        private List<Reference>? References { get; set; }  
        private List<ProductsDetail>? ProductsDetails { get; set; }

        private async Task LoadReferencesAsync()
        {
            var responseHttp = await Repository.GetAsync<List<Reference>>("/api/References/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            References = responseHttp.Response;
            References = References!.FindAll(r => r.MeasurementUnit!.PhysicalState.ToString() == productPhysicalState);    
            References = References!.Where(r => !ProductsDetails!.Where(pd => pd.ProductId == productlId).Select(pd => pd.ReferenceId).Contains(r.Id)).ToList();
        }

        private async Task LoadProductDetailsAsync()
        {
            var responseHttp = await Repository.GetAsync<List<ProductsDetail>>("/api/ProductsDetails/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            ProductsDetails = responseHttp.Response;
            ProductsDetails = ProductsDetails!.FindAll(pd => pd.ProductId == productlId);
        }

        private async Task ClickProductCallBack(string product)
        {
            string[] valores = product.Split(',');
            productlId = int.Parse(valores[0]);
            productName = valores[1];
            productPhysicalState = valores[2];
            await LoadProductDetailsAsync();
            await LoadReferencesAsync();
        }

        private async Task CreateAsync()
        {
            if (productlId == 0)
            {
                await SweetAlertService.FireAsync("Error", "Debes seleccionar un producto.", SweetAlertIcon.Error);
                return;
            }

            if (ReferencesIds == null)
            {
                await SweetAlertService.FireAsync("Error", "Debes seleccionar una referencia.", SweetAlertIcon.Error);
                return;
            }

            if (References == null)
            {
                await SweetAlertService.FireAsync("Error", "Ya guardaste todas las referencias para el producto", SweetAlertIcon.Error);
                return;
            }

            productsDetail.ProductId = productlId;
            foreach (var referenceId in ReferencesIds)
            {
                productsDetail.ReferenceId = referenceId;                
                var responseHttp = await Repository.PostAsync<ProductsDetail>("/api/ProductsDetails", productsDetail);
                if (responseHttp.Error)
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                    return;
                }
            }            
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            NavigationManager.NavigateTo("/productdetails");
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Referencias creados con exito");
        }
    }
}
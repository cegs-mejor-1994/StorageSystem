using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Pages.Recipes;
using StorageSystem.WEB.Repositories;
using static StorageSystem.Shared.Enums.ProductStateAndPhisical;

namespace StorageSystem.WEB.Pages.Products
{
    public partial class ProductCreate
    {
        private Product product = new() { Role = ProductRole.ProductoFinal, PhysicalState = ProductPhysicalState.Solido };
        private Recipe recipe = new();
        private RawMaterial rawMaterial = new();

        public ProductRole Role { get; set; }

        private string? supplierName { get; set; } = "Proveedor";
        private string? categoryName { get; set; } = "Categoria";

        private int supplierId { get; set; }        
        private int categoryId { get; set; }
        private int productId { get; set; } 
        private int productRecipeId { get; set; }

        [Inject] private IRepository repository { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;

        private List<Product>? products;

        public List<ProductRole> Roles { get; set; } = Enum.GetValues(typeof(ProductRole)).Cast<ProductRole>().ToList();

        public List<ProductPhysicalState> PhysicalState { get; set; } = Enum.GetValues(typeof(ProductPhysicalState)).Cast<ProductPhysicalState>().ToList();

        private async Task LoadProductsAsync()
        {
            var responseHttp = await repository.GetAsync<List<Product>>("/api/Products/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            products = responseHttp.Response;
        }

        private async Task CreateProductAsync()
        {
            if (product.Role == ProductRole.MateriaPrima || product.Role == ProductRole.Presentacion)
            {
                product.CategoryId = categoryId;                
            }
            else
            {
                product.CategoryId = null;
            }

            if (supplierId == 0 && (product.Role == ProductRole.MateriaPrima || product.Role == ProductRole.Presentacion))
            {
                await sweetAlertService.FireAsync("Error", "Debe seleccionar un proveedor para la materia prima.", SweetAlertIcon.Error);
                return;
            }

            if (categoryId == 0 && (product.Role == ProductRole.MateriaPrima || product.Role == ProductRole.Presentacion))
            {
                await sweetAlertService.FireAsync("Error", "Debe seleccionar una categoria la materia prima.", SweetAlertIcon.Error);
                return;
            }

            var responseHttp = await repository.PostAsync("/api/Products", product);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;                
            }

            await LoadProductsAsync();

            if (product.Role == ProductRole.ProductoFinal || product.Role == ProductRole.ProductoIntermedio)
            {                
                await CreateRecipeOfProductAsync();
            }

            if (product.Role == ProductRole.MateriaPrima || product.Role == ProductRole.Presentacion) {                                 
                await CreateRawMaterialAsync();
            }
            else
            {
                Return();
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

        private async Task CreateRawMaterialAsync()
        {
            GetProductIdByName(product.Name);            
            rawMaterial.ProductId = productId;
            rawMaterial.SupplierId = supplierId;            

            var responseHttp = await repository.PostAsync("/api/RawMaterials", rawMaterial);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            Return();            

            var toast = sweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado con éxito.");
        }

        private async Task CreateRecipeOfProductAsync()
        {
            GetProductIdByName(product.Name);
            recipe.ProductId = productId;
            recipe.TotalRecipe = 0; 

            if (recipe != null)
            {
                var responseHttp = await repository.PostAsync("/api/Recipes", recipe);
                if (responseHttp.Error)
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                    return;
                }
            }          
        }

        private void Return()
        {            
            navigationManager.NavigateTo("/products");
        }

        private void ClickSupplierCallBack(string supplier)
        {
            var supplierData = supplier.Split(',');
            supplierId = int.Parse(supplierData[0]);
            supplierName = supplierData[1];
        }

        private void ClickCategoryCallBack(string category)
        {
            var categoryData = category.Split(',');
            categoryId = int.Parse(categoryData[0]);
            categoryName = categoryData[1];
        }

        private void GetProductIdByName(string productName)
        {
            var productData = products?.FirstOrDefault(p => p.Name == productName);
            if (productData != null)
            {
                productId = productData.Id;
            }
        }
    }
}
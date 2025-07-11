using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;

namespace StorageSystem.WEB.Pages.References
{
    public partial class ReferenceCreate
    {
        private Reference reference = new();
        
        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;

        //private string typeReferenceProductName = "Producto de Referencia";
        private List<TypeReferenceProduct>? typeReferenceProducts { get; set; }
        private int typeReferenceProductId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadTypeReferenceProductsAsync();
        }

        private async Task CreateAsync()
        {
            var responseHttp = await Repository.PostAsync("/api/References", reference);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            await BlazoredModal.CloseAsync(ModalResult.Ok());
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado con éxito.");
        }

        private async Task LoadTypeReferenceProductsAsync()
        {
            var responseHttp = await Repository.GetAsync<List<TypeReferenceProduct>>("/api/TypeReferenceProducts/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            typeReferenceProducts = responseHttp.Response!;
        }

        /*void ClickTypeReferenceProductCallBack(string typeReferenceProduct)
        {
            typeReferenceProductId = int.Parse(typeReferenceProduct);
            typeReferenceProductName = GetTypeReferenceProductName(typeReferenceProductId);
        }

        private string GetTypeReferenceProductName(int id)
        {
            var typeReferenceProduct = typeReferenceProducts!.FirstOrDefault(x => x.Id == id);
            return typeReferenceProduct!.Name;
        }*/
    }
}
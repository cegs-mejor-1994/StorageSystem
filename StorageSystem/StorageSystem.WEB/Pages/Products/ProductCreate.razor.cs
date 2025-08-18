using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;

namespace StorageSystem.WEB.Pages.Products
{
    public partial class ProductCreate
    {
        private Product product = new();   
        private List<int> ListIdsReferences { get; set; } = new();
        private List<Reference> references { get; set; } = new();        
        private string productName { get; set; } = string.Empty;
        private string productCode { get; set; } = string.Empty;

        [Inject] private IRepository repository { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;

        private void ClickReferenceCallBack(List<int> listRef)
        {
            ListIdsReferences.Clear();
            if (listRef != null && listRef.Count > 0)
            {
                ListIdsReferences.AddRange(listRef);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadReferencesAsync();
        }

        private async Task CreateAsync()
        {
            var result = await sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmacion",
                Text = $"¿Estas seguro que quieres crear el product: {productName} con su codigo respectivo: {productCode}?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
            });

            try
            {
                if (ListIdsReferences.Count > 0)
                {
                    foreach (var referenceId in ListIdsReferences)
                    {
                        product.Code = productCode + GetReferenceName(referenceId);
                        product.Name = productName + " " + GetMeasurementAndReferenceName(referenceId);
                        product.ReferenceId = referenceId;
                        var responseHttp = await repository.PostAsync("/api/Products", product);
                        if (responseHttp.Error)
                        {
                            var message = await responseHttp.GetErrorMessageAsync();
                            await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                            return;
                        }                              
                    }
                    navigationManager.NavigateTo("/products");
                    var toast = sweetAlertService.Mixin(new SweetAlertOptions
                    {
                        Toast = true,
                        Position = SweetAlertPosition.BottomEnd,
                        ShowConfirmButton = true,
                        Timer = 3000
                    });
                    await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado con éxito.");
                } else
                {
                    await sweetAlertService.FireAsync("Error", "Debes elegir por lo menos una referencia", SweetAlertIcon.Error);
                }
            }
            catch (Exception ex)
            {
                await sweetAlertService.FireAsync("Error", ex.Message, SweetAlertIcon.Error);
                return;
            }
        }

        private async Task LoadReferencesAsync()
        {
            var responseHttp = await repository.GetAsync<List<Reference>>("/api/References/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            references = responseHttp.Response!;
        }

        private string GetMeasurementAndReferenceName(int referenceid)
        {
            var reference = references!.FirstOrDefault(x => x.Id == referenceid);
            return reference!.MeasurementUnit!.Name;
        }

        private string GetReferenceName(int referenceid)
        {
            var reference = references!.FirstOrDefault(x => x.Id == referenceid);
            return reference!.Name;
        }
    }
}
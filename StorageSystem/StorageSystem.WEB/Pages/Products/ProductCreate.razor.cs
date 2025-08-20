using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;
using StorageSystem.WEB.Shared;

namespace StorageSystem.WEB.Pages.Products
{
    public partial class ProductCreate
    {
        private Product product = new();
        private FormWithFields<Product>? productForm;
        //private List<int> ListIdsReferences { get; set; } = new();
        //private string ListReferencesNames { get; set; } = string.Empty;
        //private List<Reference> references { get; set; } = new();        
        //private string productName { get; set; } = string.Empty;
        //private string productCode { get; set; } = string.Empty;

        [Inject] private IRepository repository { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;

        private async Task CreateAsync()
        {
            //measurementUnit.DateRegister = DateTime.Now;
            var responseHttp = await repository.PostAsync("/api/Products", product);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
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
        }

        private void Return()
        {
            productForm!.FormPostedSuccessfully = true;
            navigationManager.NavigateTo("/products");
        }
        /*private void ClickReferenceCallBack(List<int> listRef)
        {
            ListIdsReferences.Clear();
            if (listRef != null && listRef.Count > 0)
            {
                ListIdsReferences.AddRange(listRef);
            }
        }*/

        /*protected override async Task OnInitializedAsync()
        {
            await LoadReferencesAsync();
        }*/

        /*private async Task CreateAsync()
        {           
            try
            {
                if (!string.IsNullOrEmpty(productCode) && !string.IsNullOrWhiteSpace(productName))
                {
                    if (ListIdsReferences.Count > 0)
                    {
                        foreach (var referenceId in ListIdsReferences)
                        {                           
                            ListReferencesNames += productName + " " + GetMeasurementAndReferenceName(referenceId) + ", ";
                        }
                        ListReferencesNames = ListReferencesNames.TrimEnd(',', ' ');

                        var result = await sweetAlertService.FireAsync(new SweetAlertOptions
                        {
                            Title = "Confirmacion",
                            Text = $"¿Estas seguro que quieres crear los siguientes productos: {ListReferencesNames}?",
                            Icon = SweetAlertIcon.Question,
                            ShowCancelButton = true,
                        });

                        var confirm = string.IsNullOrEmpty(result.Value);
                        if (confirm)
                        {
                            return;
                        }

                        foreach (var referenceId in ListIdsReferences)
                        {
                            product.Code = productCode + GetReferenceName(referenceId);
                            product.Name = productName + " " + GetMeasurementAndReferenceName(referenceId);
                            //product.ReferenceId = referenceId;
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
                    }
                    else
                    {
                        await sweetAlertService.FireAsync("Error", "Debes elegir por lo menos una referencia", SweetAlertIcon.Error);
                    }
                }
                else
                {
                    await sweetAlertService.FireAsync("Error", "El nombre y el código del producto son obligatorios.", SweetAlertIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                await sweetAlertService.FireAsync("Error", ex.Message, SweetAlertIcon.Error);
                return;
            }
        }*/

        /*private async Task LoadReferencesAsync()
        {
            var responseHttp = await repository.GetAsync<List<Reference>>("/api/References/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            references = responseHttp.Response!;
        }*/

        /*private string GetMeasurementAndReferenceName(int referenceid)
        {
            var reference = references!.FirstOrDefault(x => x.Id == referenceid);
            return reference!.MeasurementUnit!.Name;
        }

        private string GetReferenceName(int referenceid)
        {
            var reference = references!.FirstOrDefault(x => x.Id == referenceid);
            return reference!.Name;
        }*/
    }
}
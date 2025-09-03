using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Pages.Recipes;
using StorageSystem.WEB.Repositories;
using System.Net;

namespace StorageSystem.WEB.Pages.AppearanceReferences
{
    public partial class AppearanceReferenceDetails
    {
        [EditorRequired, Parameter] public int ReferenceId { get; set; }        

        private string rawMaterialName = "Materia prima";

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;

        private List<AppearanceReference>? AppearanceReferences { get; set; }        
        private List<RawMaterial>? rawMaterials { get; set; } = null!;

        private AppearanceReference appearanceReference = new();
        private Reference reference = new();        

        private int rawMaterialId { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            var responseHttp = await Repository.GetAsync<Reference>($"/api/References/{ReferenceId}");
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
                reference = responseHttp.Response!;
            }
            await LoadAppearanceReferencesAsync(ReferenceId);
            await LoadRawMaterialsAsync();
        }

        private void ClickRawMaterialCallBack(string rawMaterial)
        {
            string[] valores = rawMaterial.Split(',');
            rawMaterialId = int.Parse(valores[0]);
            rawMaterialName = valores[1] + valores[2];
        }

        private async Task LoadAppearanceReferencesAsync(int referenceId)
        {
            var responseHttp = await Repository.GetAsync<List<AppearanceReference>>("/api/AppearanceReferences/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            AppearanceReferences = responseHttp.Response;
            AppearanceReferences = AppearanceReferences!.Where(r => r.ReferenceId == referenceId).ToList();
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

        private async Task DeleteAsync(AppearanceReference appearanceReference)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmacion",
                Text = $"¿Estas seguro que quieres borrar la materia prima de presentacion: {appearanceReference.RawMaterial!.Name} de la Referencia?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
            });

            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }
            var responseHttp = await Repository.DeleteAsync<AppearanceReference>($"api/AppearanceReferences/{appearanceReference.Id}");
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
            await LoadAppearanceReferencesAsync(ReferenceId);
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000,
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro borrado con exito");
        }

        private async Task CreateAsync()
        {
            try
            {
                if (rawMaterialId != 0 && ReferenceId != 0)
                {
                    appearanceReference.ReferenceId = ReferenceId;
                    appearanceReference.RawMaterialId = rawMaterialId;
                    var responseHttp = await Repository.PostAsync("/api/AppearanceReferences", appearanceReference);
                    if (responseHttp.Error)
                    {
                        var message = await responseHttp.GetErrorMessageAsync();
                        await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                        return;
                    }
                    await LoadAppearanceReferencesAsync(ReferenceId);
                    rawMaterialName = "Materia prima";

                    appearanceReference = new AppearanceReference();
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
        }
    }
}
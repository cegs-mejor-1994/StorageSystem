using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using Microsoft.AspNetCore.Components.Routing;

namespace StorageSystem.WEB.Pages.Products
{
    public partial class ProductForm
    {
        private EditContext editContext = null!;

        [EditorRequired, Parameter] public Product Product { get; set; } = default!;
        [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
        [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }
        [Inject] public SweetAlertService SweetAlertService { get; set; } = null!;
        
        public bool FormPostedSuccessfully { get; set; }

        protected override void OnInitialized()
        {
            editContext = new(Product);
        }

        private async Task OnBeforeInternalNavigation(LocationChangingContext context)
        {
            var formWasEdited = editContext.IsModified();
            if (!formWasEdited || FormPostedSuccessfully)
            {
                return;
            }

            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmacion",
                Text = "¿Estás seguro de que quieres salir sin guardar los cambios?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
            });

            var confirm = !string.IsNullOrEmpty(result.Value);

            if (confirm)
            {
                return;
            }

            context.PreventNavigation();
        }
    }
}
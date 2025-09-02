using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;

namespace StorageSystem.WEB.Pages.References
{
    public partial class ReferencesListSelect
    {
        private List<Reference>? references { get; set; }
        [Parameter] public EventCallback<string> OnSelectedReferenceChanged { get; set; }

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await LoadProductsAsync();
        }

        private async Task LoadProductsAsync()
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
    }
}
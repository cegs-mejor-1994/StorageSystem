using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Pages.Categories;
using StorageSystem.WEB.Repositories;

namespace StorageSystem.WEB.Pages.Clients
{
    public partial class ClientCreate
    {
        private Client client = new();
        private Client? clientEdited;
        private ClientForm? clientForm;

        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;
        [Inject] private IRepository repository { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;

        private async Task CreateAsync()
        {
            var responseHttp = await repository.GetAsync<int>($"/api/Clients/GetClientById?CNit={client.Nit}");
            switch (responseHttp.Response)
            {
                case 0:
                    await AddClient(client);
                    break;

                case -1:
                    await sweetAlertService.FireAsync("Error", "Ya existe un registro activo con ese nit", SweetAlertIcon.Error);
                    break;

                case > 0:
                    var result = await sweetAlertService.FireAsync(new SweetAlertOptions
                    {
                        Title = "¿Desea reactivar el registro?",
                        Text = "Ya existe un registro eliminado con ese nit. ¿Desea reactivarlo?",
                        Icon = SweetAlertIcon.Question,
                        ShowCancelButton = true,
                        ConfirmButtonText = "Sí, reactivar",
                        CancelButtonText = "No, cancelar"
                    });
                    if (result.IsConfirmed)
                    {
                        clientEdited = new Client
                        {
                            Id = responseHttp.Response,
                            Code = client.Code,
                            Nit = client.Nit,
                            Address = client.Address,
                            Phone = client.Phone,
                            City = client.City,
                            State = "Disponible"
                        };
                        await UpdateClient(clientEdited);
                    }
                    break;
                default:
                    var message = await responseHttp.GetErrorMessageAsync();
                    await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                    break;
            }
        }

        private void Return()
        {
            clientForm!.FormPostedSuccessfully = true;
            navigationManager.NavigateTo("/clients");
        }

        private async Task Message(string message)
        {
            await BlazoredModal.CloseAsync(ModalResult.Ok());
            var toast = sweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: message);
        }

        private async Task AddClient(Client client)
        {
            var responseHttp = await repository.PostAsync("/api/Clients", client);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            Return();
            await Message("Registro creado con éxito.");
        }

        private async Task UpdateClient(Client client)
        {
            var responseHttpPut = await repository.PutAsync($"/api/Clients/", client);
            if (responseHttpPut.Error)
            {
                var messagePut = await responseHttpPut.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", messagePut, SweetAlertIcon.Error);
                return;
            }
            Return();
            await Message("Registro reactivado con éxito.");
        }
    }
}
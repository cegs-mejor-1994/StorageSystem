using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;
using StorageSystem.WEB.Shared;

namespace StorageSystem.WEB.Pages.Categories
{
    public partial class CategoryCreate
    {
        private Category category = new();
        private Category? categoryEdited;
        private FormWithFields<Category>? categoryForm;

        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;
        [Inject] private IRepository repository { get; set; } = null!;
        [Inject] private SweetAlertService sweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager navigationManager { get; set; } = null!;

        private async Task CreateAsync()
        {
            var responseHttp = await repository.GetAsync<int>($"/api/Categories/GetCategoryById?CCode={category.Code}&CName={category.Name}");
            switch (responseHttp.Response)
            {
                case 0:
                    await AddCategory(category);                    
                    break;

                case -1:
                    await sweetAlertService.FireAsync("Error", "Ya existe un registro activo con ese codigo y con ese nombre", SweetAlertIcon.Error);
                    break;

                case > 0:
                    var result = await sweetAlertService.FireAsync(new SweetAlertOptions
                    {
                        Title = "¿Desea reactivar el registro?",
                        Text = "Ya existe un registro eliminado con ese código o nombre. ¿Desea reactivarlo?",
                        Icon = SweetAlertIcon.Question,
                        ShowCancelButton = true,
                        ConfirmButtonText = "Sí, reactivar",
                        CancelButtonText = "No, cancelar"
                    });
                    if (result.IsConfirmed)
                    {
                        categoryEdited = new Category
                        {
                            Id = responseHttp.Response,
                            Code = category.Code,
                            Name = category.Name,                            
                            State = "Disponible"
                        };
                        await UpdateCategory(categoryEdited);
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
            categoryForm!.FormPostedSuccessfully = true;
            navigationManager.NavigateTo("/categories");
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

        private async Task AddCategory(Category category)
        {
            var responseHttp = await repository.PostAsync("/api/Categories", category);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await sweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            Return();
            await Message("Registro creado con éxito.");
        }

        private async Task UpdateCategory(Category category)
        {
            var responseHttpPut = await repository.PutAsync($"/api/Categories/", category);
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
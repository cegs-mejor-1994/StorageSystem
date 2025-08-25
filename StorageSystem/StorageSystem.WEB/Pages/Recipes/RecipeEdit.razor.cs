using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Pages.Categories;
using StorageSystem.WEB.Repositories;

namespace StorageSystem.WEB.Pages.Recipes
{
    public partial class RecipeEdit
    {
        [EditorRequired, Parameter] public int ProductId { get; set; }

        [CascadingParameter] IModalService Modal { get; set; } = default!;

        private string? productName;        

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;

        private List<Recipe>? Recipes { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            await LoadRecipesAsync(ProductId);
            GetProductName();            
        }

        private async Task ShowModalAsync(int id)
        {
            IModalReference modalReference;
            modalReference = Modal.Show<RecipeEditById>(string.Empty, new ModalParameters().Add("Id", id));
 
            var result = await modalReference.Result;
            if (result.Confirmed)
            {
                await LoadRecipesAsync(ProductId);
            }
        }

        private async Task DeleteAsync(Recipe recipe)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmacion",
                Text = $"¿Estas seguro que quieres borrar la materia prima: {recipe.RawMaterial!.Name} de la formula?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
            });

            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }
            var responseHttp = await Repository.DeleteAsync<Recipe>($"api/Recipes/{recipe.Id}");
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
            await LoadRecipesAsync(ProductId);
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000,
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro borrado con exito");
        }

        private async Task LoadRecipesAsync(int productId)
        {
            var responseHttp = await Repository.GetAsync<List<Recipe>>("/api/Recipes/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            Recipes = responseHttp.Response;
            Recipes = Recipes!.Where(r => r.ProductId == productId).ToList();
        }

        private string GetProductName()
        {
            if (Recipes != null && Recipes.Count > 0)
            {
                productName = Recipes[0].Product?.Name;
            }
            return productName ?? string.Empty;
        }              
    }
}
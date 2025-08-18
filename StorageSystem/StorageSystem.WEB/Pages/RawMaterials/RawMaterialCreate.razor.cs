using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;
using StorageSystem.WEB.Repositories;

namespace StorageSystem.WEB.Pages.RawMaterials
{
    public partial class RawMaterialCreate
    {
        private RawMaterial rawMaterial = new();
        private string measurementUnitName = "Unidad de Medida";
        private string categoryName = "Categoria";

        private int categoryId { get; set; }
        private int measurementUnitId { get; set; }

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;


        private void ClickCategoryCallBack(string category)
        {
            string[] valores = category.Split(',');
            categoryId = int.Parse(valores[0]);
            categoryName = valores[1];
        }

        private void ClickMeasurementUnitCallBack(string measurementUnit)
        {
            string[] valores = measurementUnit.Split(',');
            measurementUnitId = int.Parse(valores[0]);
            measurementUnitName = valores[1];
        }

        private async Task CreateAsync()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(rawMaterial.Name) && categoryId != 0 && measurementUnitId != 0)
                {
                    rawMaterial.CategoryId = categoryId;
                    rawMaterial.MeasurementUnitId = measurementUnitId;
                    var responseHttp = await Repository.PostAsync("/api/RawMaterials", rawMaterial);
                    if (responseHttp.Error)
                    {
                        var message = await responseHttp.GetErrorMessageAsync();
                        await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                        return;
                    }
                    NavigationManager.NavigateTo("/rawmaterials");
                    var toast = SweetAlertService.Mixin(new SweetAlertOptions
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
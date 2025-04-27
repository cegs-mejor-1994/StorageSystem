using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;

namespace StorageSystem.WEB.Pages.Suppliers
{
    public partial class SuppliersListSelect
    {
        [CascadingParameter] List<Supplier>? suppliers { get; set; }
        [Parameter] public EventCallback<string> OnSelectedSupplierChanged { get; set; }
    }
}
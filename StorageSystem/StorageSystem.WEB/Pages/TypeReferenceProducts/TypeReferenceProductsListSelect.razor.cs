using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;

namespace StorageSystem.WEB.Pages.TypeReferenceProducts
{
    public partial class TypeReferenceProductsListSelect
    {
        [CascadingParameter] List<TypeReferenceProduct>? typeReferenceProducts { get; set; }
        [Parameter] public EventCallback<string> OnSelectedTypeReferenceProductChanged { get; set; }
    }
}
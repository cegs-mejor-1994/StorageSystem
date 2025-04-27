using Microsoft.AspNetCore.Components;
using StorageSystem.Shared.Entities;

namespace StorageSystem.WEB.Pages.RawMaterials
{
    public partial class RawMaterialsListSelect
    {
        [CascadingParameter] List<RawMaterial>? rawMaterials { get; set; }
        [Parameter] public EventCallback<string> OnSelectedRawMaterialChanged { get; set; }        
    }
}
using System.ComponentModel;

namespace StorageSystem.Shared.Enums
{
    public enum SolidReferenceType
    {
        [Description("100 g")]
        S100g,
        [Description("250 g")]
        S250g,
        [Description("500 g")]
        S500g,
        [Description("1000 g")]
        S1000g,
        [Description("5 Libras")]
        S2500g,
        [Description("5 Kilos")]
        S5000g,
        [Description("10 Kilos")]
        S10000g,
        [Description("Arroba")]
        S12500g,
        [Description("15 Kilos")]
        S15000g
    }
}
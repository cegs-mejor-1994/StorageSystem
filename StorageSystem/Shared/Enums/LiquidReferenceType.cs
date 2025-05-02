using System.ComponentModel;

namespace StorageSystem.Shared.Enums
{
    public enum LiquidReferenceType
    {
        [Description("60ml")]
        L60ml,
        [Description("95ml")]
        L95ml,
        [Description("D.P. 100ml")]
        Ldp100ml,
        [Description("110ml")]
        L110ml,
        [Description("165ml")]
        L165ml,
        [Description("250ml")]
        L250ml,
        [Description("D.P. 250ml")]
        Ldp250ml,
        [Description("500ml")]
        L500ml,
        [Description("1000ml")]
        L1000ml,
        [Description("3000ml")]
        L3000ml,
        [Description("5000ml")]
        L5000ml,
        [Description("20000ml")]
        L20000ml,
        [Description("30000ml")]
        L30000ml,
    }
}

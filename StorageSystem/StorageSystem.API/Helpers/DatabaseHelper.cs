using Microsoft.EntityFrameworkCore;

namespace StorageSystem.API.Helpers
{
    public class DatabaseHelper
    {
        private static readonly Dictionary<string, string> FieldTranslations = new()
        {
            // 🔹 Traduce nombres de entidades
            { "MeasurementUnits", "Unidad de medida" },
            { "Products", "Producto" },
            { "Categories", "Categoría" },
            { "MeasurementConversions", "Conversion de medida" },
            { "ProductsDetails", "Presentacion" },
            { "ProductsDetailStructures", "Estructura" },
            { "RawMaterials", "Materia Prima" },
            { "Recipes", "Formula" },
            { "RecipeDetail", "Detalle Formula" },
            { "References", "Referencia" },
            { "Supplier", "Proveedor" },

            // 🔹 Traduce nombres de campos
            { "Code", "Código" },
            { "Name", "Nombre" },
            { "Description", "Descripción" },
            { "Email", "Correo electrónico" },
            { "Phone", "Teléfono" },
            { "Username", "Usuario" },
        };

        public static string GetFriendlyUniqueConstraintError(DbUpdateException ex)
        {
            if (ex.InnerException == null)
                return "Error de base de datos al guardar los cambios.";

            string message = ex.InnerException.Message;

            // Detectar índice único (ejemplo: IX_MeasurementUnits_Code)
            if (message.Contains("IX_"))
            {
                int start = message.IndexOf("IX_");
                int end = message.IndexOf("'", start);
                string indexName = message.Substring(start, end - start);

                // Limpiar nombre técnico (IX_MeasurementUnits_Code → MeasurementUnits Code)
                string fieldName = indexName
                    .Replace("IX_", "")
                    .Replace("_", " ");

                // Traducir automáticamente a nombres amigables
                var parts = fieldName.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

                string entity = parts.Length > 0 ? parts[0] : "registro";
                string field = parts.Length > 1 ? parts[1] : "campo";

                // Traducir automáticamente
                entity = FieldTranslations.ContainsKey(entity) ? FieldTranslations[entity] : entity;
                field = FieldTranslations.ContainsKey(field) ? FieldTranslations[field] : field;

                return $"❌ Ya existe un registro con el mismo {field} en {entity}.";
            }

            // Clave primaria duplicada
            if (message.Contains("PRIMARY KEY"))
                return "❌ El registro ya existe (clave primaria duplicada).";

            return $"❌ Error de base de datos: {message}";
        }
    }
}

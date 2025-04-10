﻿using StorageSystem.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace StorageSystem.Shared.Entities
{
    public class Category : IEntityFields
    {
        public int Id { get; set; }

        [Display(Name = "Codigo")]
        [MaxLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Code { get; set; } = null!;

        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Name { get; set; } = null!;
        public string State { get; set; } = "Disponible";
        public DateTime DateRegister { get; set; }

        public ICollection<RawMaterial>? RawMaterials { get; set; }
    }
}

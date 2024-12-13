using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proyectofinal1_.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [StringLength(50, ErrorMessage = "Este campo debe tener máximo 50 caracteres.")]
        [Display(Name = "Nombre de usuario")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Este campo debe tener máximo 50 caracteres.")]
        [Display(Name = "Contraseña")]
        public string ContraseñaHash { get; set; }
        [HiddenInput]
        public byte[] HashKey { get; set; }
        [HiddenInput]
        public byte[] HashIV { get; set; }
        [ScaffoldColumn(false)]
        public DateTime FechaCreacion { get; set; }
    }
}
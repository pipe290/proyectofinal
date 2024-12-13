using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proyectofinal1_.Models
{
    public class Persona
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre no puede estar vacío.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "máximo 50 caracteres.")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "El apellido no puede estar vacío.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "máximo 50 caracteres.")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El número de identificación no puede estar vacío.")]
        [StringLength(10, MinimumLength = 5, ErrorMessage = "Este campo debe tener máximo 10 caracteres")]
        [Display(Name = "Número de identificación")]
        public string NumeroIdentificacion { get; set; }

        [Required(ErrorMessage = "El correo electrónico no puede estar vacío.")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Este campo debe tener entre 10 y 50 caracteres.")]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El documento no puede estar vacío.")]
        [Display(Name = "Documento")]
        public string tipodocumento { get; set; }

        public DateTime FechaDeCreacion { get; set; }

    }
}
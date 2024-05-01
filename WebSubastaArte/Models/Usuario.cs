using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebSubastaArte.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; } 
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Clave { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string ConfirmarClave { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public DateTime FechaNacimiento { get; set; }
        [Required]
        public string Genero { get; set; }
        [Required] 
        public string TipoUsuario { get; set; } 

        [Display(Name = "Imagen")]
        public byte[] Imagen { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFitnessProy1.Models
{
    public class Usuario
    {
        public int Id { get; set; } 
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
        public int Altura { get; set; } //en centimetros
        [Required]
        public decimal PesoActual { get; set; } //en kilogramos
        [Required]
        public string Genero { get; set; }

        [Display(Name = "Imagen")]
        public byte[] Imagen { get; set; }
    }
}
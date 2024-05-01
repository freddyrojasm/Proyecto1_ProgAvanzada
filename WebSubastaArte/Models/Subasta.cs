using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebSubastaArte.Models
{
    public class Subasta
    {
        public int Id { get; set; }

        [Required]
        public string PrecioInicial { get; set; }

        [Required]
        public int Duracion { get; set; }

        [Required]
        public DateTime FechaHora { get; set; }
        [Display(Name = "Imagen")]
        public byte[] Imagen { get; set; }

        public string Comentarios { get; set; }
    }
}
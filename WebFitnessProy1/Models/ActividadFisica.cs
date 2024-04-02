using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFitnessProy1.Models
{
    public class ActividadFisica
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string TipoActividad { get; set; }

        [Required]
        public DateTime FechaHora { get; set; }

        [Required]
        public int Duracion { get; set; }

        public int DistanciaRecorrida { get; set; } //kilometros

        public int CaloriasQuemadas { get; set; }

        public string Comentarios { get; set; }

    }
}
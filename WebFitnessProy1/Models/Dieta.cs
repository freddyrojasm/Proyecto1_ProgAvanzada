using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFitnessProy1.Models
{
    public class Dieta
    {   public int Id { get; set; }
        public string Email { get; set; }
        public DateTime FechaComida { get; set; }
        public string TipoComida { get; set; }
        public string AlimentosConsumidos { get; set; }
        public int CaloriasTotales { get; set; }
        public string Comentarios { get; set; }
    }
}
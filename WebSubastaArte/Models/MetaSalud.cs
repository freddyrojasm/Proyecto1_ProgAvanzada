using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFitnessProy1.Models
{
    public class MetaSalud
    {
        public int IdMeta { get; set; }
        public string Email { get; set; }
        public string TipoMeta { get; set; }
        public decimal PesoObjetivo { get; set; }
        public DateTime FechaObjetivo { get; set; }
        public string NivelActividadDes { get; set; }
        public string ObjEspecificos { get; set; }
    }
}
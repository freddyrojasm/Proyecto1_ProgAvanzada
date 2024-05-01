using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebSubastaArte.Models
{
    public class ObraArte
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "La categoría es obligatoria")]
        public string Categoria { get; set; }
        public int DimensionLargo { get; set; }
        public int DimensionAncho { get; set; }

        [Display(Name = "Imagen")]
        public byte[] Imagen { get; set; }

        public int IdUsuario { get; set; } //id de usuario que carga la obra
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VentaArticulosCredito.Models.Articulo
{
    public class CreateArticleModel
    {
        /// <summary>
        /// Nombre artículo
        /// </summary>
        [Required(ErrorMessage = "Ingrese nombre")]
        [DataType(DataType.Text)]
        public string nombre { get; set; }

        /// <summary>
        /// Subcategoría
        /// </summary>
        [Required(ErrorMessage = "Ingrese descipción")]
        [DataType(DataType.Text)]
        public string descripcion { get; set; }

        /// <summary>
        /// Categoría
        /// </summary>
        [Required(ErrorMessage = "Seleccione categoria")]
        [DataType(DataType.Text)]
        public int categoria { get; set; }

        /// <summary>
        /// Subcategoría
        /// </summary>
        [Required(ErrorMessage = "Seleccione sub categoria")]
        [DataType(DataType.Text)]
        public int subCategoria { get; set; }
    }
}
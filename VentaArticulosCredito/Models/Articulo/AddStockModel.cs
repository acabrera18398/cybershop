using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VentaArticulosCredito.Models.Articulo
{
    public class AddStockModel
    {
        /// <summary>
        /// Código del articulo
        /// </summary>
        public int articulo { get; set; }

        /// <summary>
        /// Precio de compra
        /// </summary>
        [Required(ErrorMessage = "Ingrese precio de compra")]
        [DataType(DataType.Text)]
        public double precioCompra { get; set; }

        /// <summary>
        /// Precio de compra
        /// </summary>
        [Required(ErrorMessage = "Ingrese precio de venta")]
        [DataType(DataType.Text)]
        public double precioVenta { get; set; }

        /// <summary>
        /// Precio de compra
        /// </summary>
        [Required(ErrorMessage = "Ingrese cantidad")]
        [DataType(DataType.Text)]
        public int cantidad { get; set; }

        /// <summary>
        /// Precio de compra
        /// </summary>
        [Required(ErrorMessage = "Ingrese fecha de compra")]
        [DataType(DataType.DateTime)]
        public DateTime fecha { get; set; }
    }
}
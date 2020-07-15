using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VentaArticulosCredito.Models.Carrito
{
    public class CarritoPageModel
    {
        /// <summary>
        /// Constructor con parámetros
        /// </summary>
        public CarritoPageModel(List<VentaArticulosCredito.DBEntities.Carrito> carrito, List<VentaArticulosCredito.DBEntities.Direccion_Usuario> direcciones)
        {
            this.carrito = carrito;
            this.direcciones = direcciones;
        }

        /// <summary>
        /// Carrito del usuario
        /// </summary>
        public List<VentaArticulosCredito.DBEntities.Carrito> carrito { get; set; }

        /// <summary>
        /// Listado de direcciones del usuario
        /// </summary>
        public List<VentaArticulosCredito.DBEntities.Direccion_Usuario> direcciones { get; set; }
    }
}
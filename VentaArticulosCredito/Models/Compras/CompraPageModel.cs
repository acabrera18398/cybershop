using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VentaArticulosCredito.DBEntities;

namespace VentaArticulosCredito.Models.Compras
{
    public class CompraPageModel
    {
        /// <summary>
        /// Orden que se desea ver
        /// </summary>
        public Factura factura { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VentaArticulosCredito.DBEntities;

namespace VentaArticulosCredito.Models.Compras
{
    public class ComprasPageModel
    {
        /// <summary>
        /// Lista de compras
        /// </summary>
        public List<Factura> compras { get; set; }
    }
}
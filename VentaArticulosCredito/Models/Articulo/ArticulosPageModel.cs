using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VentaArticulosCredito.Models.Articulo
{
    public class ArticulosPageModel
    {
        /// <summary>
        /// Lista de artículos
        /// </summary>
        public List<VentaArticulosCredito.DBEntities.Articulo> articulos { get; set; }

        /// <summary>
        /// Modelo para vista agregar stock
        /// </summary>
        public AddStockModel stockModel { get; set; }

        /// <summary>
        /// Modelo vista crear atículo
        /// </summary>
        public CreateArticleModel createModel { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VentaArticulosCredito.DBEntities;

namespace VentaArticulosCredito.Models.Inicio
{
    public class InicioPageModel
    {
        /// <summary>
        /// Constructor con parámetros
        /// </summary>
        /// <param name="articulos">Articulos</param>
        /// <param name="categorias">Categorias</param>
        /// <param name="subCategorias">Subcategorias</param>
        public InicioPageModel(List<Inventario> articulos, List<Categoria> categorias)
        {
            this.articulos = articulos;
            this.categorias = categorias;
        }

        /// <summary>
        /// Constructor vacío
        /// </summary>
        public InicioPageModel()
        {

        }

        /// <summary>
        /// Articulos disponibles para su venta
        /// </summary>
        public List<Inventario> articulos { get; set; }

        /// <summary>
        /// Lista de categorias
        /// </summary>
        public List<Categoria> categorias { get; set; }

        /// <summary>
        /// Articulo seleccionado para ver más información
        /// </summary>
        public Inventario articulo { get; set; }
    }
}
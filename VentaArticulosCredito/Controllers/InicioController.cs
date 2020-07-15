using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VentaArticulosCredito.DBEntities;
using VentaArticulosCredito.Helpers;
using VentaArticulosCredito.Models.Inicio;
using Microsoft.Ajax.Utilities;

namespace VentaArticulosCredito.Controllers
{
    public class InicioController : ControlController
    {
        /// <summary>
        /// Devuelve la vista principal de la página
        /// </summary>
        /// <returns>Vista</returns>
        [HttpGet]
        public ActionResult Inicio(string filter, string subCategoria)
        {
            if (ValidaUsuario())
            {
                using (var db = new VentaArticulosCreditoEntities())
                {
                    List<Categoria> categorias = db.Categoria.Include("SubCategoria").ToList();
                    List<Inventario> stock = db.Inventario.Include("Articulo.Imagen_Articulo").Where(i => i.cantidad > 0).OrderBy(i => i.fechaCompra).DistinctBy(i => i.codigoArticulo).ToList();

                    var model = new InicioPageModel(stock, categorias);

                    return View(model);
                }
            }

            return RedirectToAction("Login","Login");
        }

        /// <summary>
        /// Elimina las variables de sesión del usuario logueado
        /// </summary>
        /// <returns>Json con tipo y mensaje</returns>
        [HttpGet]
        public JsonResult CloseSession()
        {
            int tipo = 0;
            string mensaje = "";

            try
            {
                Session.Clear();
                Session.Abandon();
                Session.RemoveAll();
                tipo = 1;
            }
            catch (Exception ex)
            {
                mensaje = "Error al cerrar sesión";
            }

            return Json(new { tipo = tipo, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retorna la infomación del inventario que se desea ver
        /// </summary>
        /// <param name="correlativoInventario">Codigo inventario</param>
        /// <returns>Modelo</returns>
        [HttpGet]
        public ActionResult ArticuloModalPartialView(int correlativoInventario)
        {
            Inventario model = null;

            using (var db = new VentaArticulosCreditoEntities())
            {
                model = db.Inventario.Include("Articulo.Imagen_Articulo").Where(i => i.correlativo == correlativoInventario).FirstOrDefault();
            }

            return PartialView("ArticuloModalPartialView", model);
        }

        /// <summary>
        /// Devuelve la cantidad de artículos que tiene el carrito del usuario logueado
        /// </summary>
        /// <returns>Json</returns>
        [HttpGet]
        public JsonResult ActualizarIndicadorCarrito()
        {
            int tipo = 0;
            string mensaje = "";
            int cantidad = 0;

            using (var db = new VentaArticulosCreditoEntities())
            {
                try
                {
                    var user = (Usuario)Session["usuario"];
                    var carrito = db.Carrito.Where(c => c.codigoUsuario == user.codigo).ToList();

                    cantidad = carrito.Count;
                    tipo = 1;
                }
                catch (Exception ex)
                {
                    mensaje = "Error al cargar información del carrito";
                }
            }

            return Json(new { tipo = tipo, mensaje = mensaje, cantidad = cantidad }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Agrega un artículo al carrito de compras
        /// </summary>
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult GuardarEnCarrito(int inventario, int cantidad)
        {
            int tipo = 0;
            string mensaje = "";
            Boolean exitoso = false;

            using (var db = new VentaArticulosCreditoEntities())
            {
                try
                {
                    var user = (Usuario)Session["usuario"];
                    var carritoExistente = db.Carrito.Where(c => c.codigoUsuario == user.codigo && c.codigoInventario == inventario).FirstOrDefault();

                    if (carritoExistente == null)
                    {
                        var carrito = new Carrito(user.codigo, inventario, cantidad);

                        db.Carrito.Add(carrito);
                        db.SaveChanges();
                        exitoso = true;
                    }
                    else
                    {
                        carritoExistente.cantidad += cantidad;
                        db.Entry(carritoExistente).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        exitoso = true;
                    }

                    if (exitoso)
                    {
                        tipo = 1;
                        mensaje = "Agregado al carrito";
                    }

                }
                catch (Exception ex)
                {
                    mensaje = "Error al agregar al carrito";
                }
            }

            return Json(new { tipo = tipo, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
    }
}
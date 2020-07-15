using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VentaArticulosCredito.DBEntities;


namespace VentaArticulosCredito.Controllers
{
    public class CarritoController : ControlController
    {
        /// <summary>
        /// Devuelve la vista del carrito de compras
        /// </summary>
        /// <returns>Vista</returns>
        [HttpGet]
        public ActionResult Carrito()
        {
            if (ValidaUsuario())
            {
                using (var db = new VentaArticulosCreditoEntities())
                {
                    var user = (Usuario)Session["usuario"];
                    var carrito = db.Carrito.Include("Inventario.Articulo.Imagen_Articulo").Where(c => c.codigoUsuario == user.codigo).ToList();
                    var direcciones = db.Direccion_Usuario.Include("Usuario").Include("Municipio.Departamento").Where(d => d.codigoUsuario == user.codigo).ToList();
                    var model = new VentaArticulosCredito.Models.Carrito.CarritoPageModel(carrito, direcciones);

                    return View(model);
                }
            }

            return RedirectToAction("Login","Login");
        }

        /// <summary>
        /// Elimina artículo del carrito
        /// </summary>
        /// <param name="correlativo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarArticulo(int correlativo)
        {
            int tipo = 0;
            string mensaje = "";

            using(var db = new VentaArticulosCreditoEntities())
            {
                try
                {
                    var carrito = db.Carrito.Where(c => c.correlativo == correlativo).FirstOrDefault();

                    if(carrito != null)
                    {
                        db.Carrito.Remove(carrito);
                        db.SaveChanges();
                        tipo = 1;
                    }
                }
                catch (Exception ex)
                {
                    mensaje = "Error al eliminar del carrito";
                }
            }

            return Json(new { tipo = tipo, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
    }
}
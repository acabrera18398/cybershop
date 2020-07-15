using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VentaArticulosCredito.DBEntities;
using VentaArticulosCredito.Models.Compras;

namespace VentaArticulosCredito.Controllers
{
    public class ComprasController : ControlController
    {
        /// <summary>
        /// Devuelve la lista con las compras realizadas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Compras()
        {
            if (ValidaUsuario())
            {
                ComprasPageModel model = new ComprasPageModel();

                using (var db = new VentaArticulosCreditoEntities())
                {
                    var user = (Usuario)Session["usuario"];
                    model.compras = db.Factura.Include("Direccion_Usuario").Include("DetalleFactura.Articulo").Where(c => c.codigoUsuario == user.codigo).ToList();
                }

                return View("ComprasListView", model);
            }

            return RedirectToAction("Login", "Login");
        }

        /// <summary>
        /// Devuelve la vista con la orden de compra que se quiere ver
        /// </summary>
        /// <param name="orden"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Compra(String orden)
        {
            if (ValidaUsuario())
            {
                CompraPageModel model = new CompraPageModel();
                int fact = 0;

                using (var db = new VentaArticulosCreditoEntities())
                {
                    if (orden != null)
                    {
                        fact = Convert.ToInt32(orden);

                        model.factura = db.Factura.Include("DetalleFactura.Articulo.Imagen_Articulo").Include("Direccion_Usuario.Municipio.Departamento").Where(f => f.codigo == fact).FirstOrDefault();
                        Session["orden-compra"] = orden;
                    }
                    else
                    {
                        fact = Convert.ToInt32(Session["orden-compra"]);

                        model.factura = db.Factura.Include("DetalleFactura.Articulo.Imagen_Articulo").Include("Direccion_Usuario.Municipio.Departamento").Where(f => f.codigo == fact).FirstOrDefault();
                    }
                }

                return View("CompraView", model);
            }

            return RedirectToAction("Login","Login");
        }

        /// <summary>
        /// Confirma la orden de compra del carrito del usuario
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ConfirmarOrden(int direccionEntrega)
        {
            int tipo = 0;
            string mensaje = "";
            int orden = 0;

            using (var db = new VentaArticulosCreditoEntities())
            {
                try
                {
                    var user = (Usuario)Session["usuario"];
                    var carrito = db.Carrito.Include("Inventario.Articulo").Where(c => c.codigoUsuario == user.codigo).ToList();
                    var correlativo = new CorrelativoFactura(Guid.NewGuid().ToString());
                    int posicion = 1;

                    //Creo el códio de la factura
                    db.CorrelativoFactura.Add(correlativo);
                    db.SaveChanges();
                    //Creo el encabezado de la factura
                    var factura = new Factura(correlativo.correlativo, DateTime.Now, user.codigo, 0, direccionEntrega);
                    db.Factura.Add(factura);
                    db.SaveChanges();

                    foreach (var car in carrito)
                    {
                        //Creo detalle por cada posición del carrito
                        var detalle = new DetalleFactura(posicion, correlativo.correlativo, (decimal)car.Inventario.precioVenta, (int)car.cantidad, car.Inventario.Articulo.codigo);
                        db.DetalleFactura.Add(detalle);
                        db.SaveChanges();
                        //Recupero el inventario de la posición y le resto la cantidad ordenada
                        var inventario = car.Inventario;
                        inventario.cantidad -= car.cantidad;
                        db.Entry(inventario).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        //Elimino la posición del carrito
                        db.Carrito.Remove(car);
                        db.SaveChanges();
                        posicion += 1;
                    }

                    orden = correlativo.correlativo;
                    //Envío correo
                    MailController().SendConfirmOrder(user, ref tipo, ref mensaje, orden);                    
                }catch(Exception ex)
                {
                    mensaje = "Error al confirmar orden";
                }
            }

            return Json(new { tipo = tipo, mensaje = mensaje, orden = orden }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Cancela la orden 
        /// </summary>
        /// <param name="orden">Número orden</param>
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult CancelarOrden(int orden)
        {
            int tipo = 0;
            string mensaje = "";

            using (var db = new VentaArticulosCreditoEntities())
            {
                try
                {
                    var order = db.Factura.Where(o => o.codigo == orden).FirstOrDefault();
                    var user = (Usuario)Session["usuario"];

                    order.estado = 4;
                    db.Entry(order).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    //Envío correo
                    MailController().SendCancelOrder(user, ref tipo, ref mensaje, orden);
                }
                catch (Exception ex)
                {
                    mensaje = "Error al cancelar orden";
                }
            }

            return Json(new { tipo = tipo, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
    }
}
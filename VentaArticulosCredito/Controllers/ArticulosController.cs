using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VentaArticulosCredito.DBEntities;
using VentaArticulosCredito.Models.Articulo;

namespace VentaArticulosCredito.Controllers
{
    public class ArticulosController : ControlController
    {
        /// <summary>
        /// Devuelve la vista de artículos
        /// </summary>
        /// <returns></returns>
        public ActionResult Articulos()
        {
            if (ValidaUsuario())
            {
                ArticulosPageModel model = new ArticulosPageModel();

                using (var db = new VentaArticulosCreditoEntities()) {
                    ViewBag.ListaCategorias = db.Categoria.ToList();
                    model.articulos = db.Articulo.Include("SubCategoria.Categoria").Include("Imagen_Articulo").ToList();
                }

                return View("ArticuloListView", model);
            }

            return RedirectToAction("Login", "Login");
        }

        /// <summary>
        /// Agrega stock al artículo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost,ValidateAntiForgeryToken]
        public JsonResult AddStock(AddStockModel model)
        {
            int tipo = 0;
            string mensaje = "";

            if (!ModelState.IsValid)
            {
                //Extrae el primer mensaje de error que tenga el modelo
                mensaje = ModelState.Values.Select(e => e.Errors).Where(e => e.Count > 0).FirstOrDefault().Select(v => v.ErrorMessage).FirstOrDefault();
            }
            else
            {
                using(var db = new VentaArticulosCreditoEntities())
                {
                    var stock = new Inventario(model.articulo, model.precioCompra, model.precioVenta, model.cantidad, model.fecha);

                    db.Inventario.Add(stock);
                    db.SaveChanges();
                    tipo = 1;
                    mensaje = "Stock agregado correctamente";
                }
            }

            return Json(new { tipo = tipo, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Crea un artículo
        /// </summary>
        /// <param name="model">Modelo</param>
        /// <returns>Json</returns>
        [HttpPost, AllowAnonymous]
        public async Task<JsonResult> CreateArticle(CreateArticleModel model, List<HttpPostedFileBase> imagenes)
        {
            int tipo = 0;
            string mensaje = "";
            Boolean imagenesValidas = true;

            if (!ModelState.IsValid)
            {
                //Extrae el primer mensaje de error que tenga el modelo
                mensaje = ModelState.Values.Select(e => e.Errors).Where(e => e.Count > 0).FirstOrDefault().Select(v => v.ErrorMessage).FirstOrDefault();
            }

            if(mensaje == "")
            {
                if (imagenes == null)
                    imagenes = new List<HttpPostedFileBase>();

                if(imagenes.Count > 0)
                {
                    foreach(var im in imagenes)
                    {
                        if (im.ContentType.Split('/')[0] != "image")
                            imagenesValidas = false;
                    }

                    if (imagenesValidas)
                    {
                        try
                        {
                            using (var db = new VentaArticulosCreditoEntities())
                            {
                                var articulo = new Articulo(model.descripcion, model.subCategoria, model.nombre);
                                db.Articulo.Add(articulo);
                                db.SaveChanges();

                                foreach (var im in imagenes)
                                {
                                    var imagen = new Imagen_Articulo(articulo.codigo);

                                    db.Imagen_Articulo.Add(imagen);
                                    db.SaveChanges();
                                    imagen.imagen = imagen.codigo + "." + im.ContentType.Split('/')[1];
                                    db.Entry(imagen).State = System.Data.Entity.EntityState.Modified;
                                    db.SaveChanges();
                                    im.SaveAs(ImagesPath() + imagen.imagen);
                                }

                                tipo = 1;
                                mensaje = "Artículo creado correctamente";
                            }
                        }
                        catch (Exception ex)
                        {
                            mensaje = "Error al crear artículo";
                        }
                    }
                    else
                    {
                        mensaje = "Sólo se permiten imágenes";
                    }
                }
                else
                {
                    mensaje = "Seleccione al menos una imágen";
                }
            }

            return Json(new { tipo = tipo, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Recupera subcategorias
        /// </summary>
        /// <param name="categoria">Categoria</param>
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult GetSubCategorias(int categoria)
        {
            int tipo = 0;
            string mensaje = "";
            SelectList listaSub = null;

            using (var db = new VentaArticulosCreditoEntities())
            {
                try
                {
                    listaSub = new SelectList(db.SubCategoria.Where(m => m.codigoCategoria == categoria).ToList(), "codigo", "nombre");
                    tipo = 1;
                }
                catch (Exception ex)
                {
                    mensaje = "Error al recuperar municipios";
                }
            }

            return Json(new { tipo = tipo, mensaje = mensaje, listaSub = listaSub }, JsonRequestBehavior.AllowGet);
        }
    }
}
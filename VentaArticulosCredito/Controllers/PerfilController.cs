using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VentaArticulosCredito.DBEntities;
using VentaArticulosCredito.Helpers;
using VentaArticulosCredito.Models.Perfil;

namespace VentaArticulosCredito.Controllers
{
    public class PerfilController : ControlController
    {
        /// <summary>
        /// Devuelve la vista del perfil
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Perfil()
        {
            if (ValidaUsuario()) {
                using (var db = new VentaArticulosCreditoEntities())
                {
                    var usuario = (Usuario)Session["usuario"];
                    ViewBag.ListaDepartamentos = db.Departamento.ToList();
                    return View("PerfilView", new PerfilPageModel(db.Usuario.Include("Direccion_Usuario.Municipio.Departamento").Where(u => u.codigo == usuario.codigo).FirstOrDefault()));
                }
            }    

            return RedirectToAction("Login", "Login");
        }

        /// <summary>
        /// Actualiza la información del usuario
        /// </summary>
        /// <param name="model">Model update</param>
        /// <returns>Json</returns>
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult UpdateInfo(UpdateInfoModel model)
        {
            int tipo = 0;
            string mensaje = "";
            string correo = "";
            Boolean validaNIT = false;
            Boolean guardar = true;
            Boolean validaCorreo = false;

            try
            {
                if (!ModelState.IsValid)
                {
                    //Extrae el primer mensaje de error que tenga el modelo
                    mensaje = ModelState.Values.Select(e => e.Errors).Where(e => e.Count > 0).FirstOrDefault().Select(v => v.ErrorMessage).FirstOrDefault();
                }
                else
                {
                    var user = (Usuario)Session["usuario"];

                    if (HuboCambios(user, model, ref validaNIT, ref validaCorreo))
                    {
                        using (var db = new VentaArticulosCreditoEntities())
                        {
                            if (validaNIT)
                            {
                                if (ValidarNIT.ValidaNIT(model.nit))
                                {
                                    var userNIT = db.Usuario.Where(u => u.nit == model.nit).FirstOrDefault();

                                    if (userNIT == null)
                                    {
                                        user.nit = model.nit;
                                    }
                                    else
                                    {
                                        guardar = false;
                                        mensaje = "Este NIT ya está siendo utilzado";
                                    }
                                }
                                else
                                {
                                    guardar = false;
                                    mensaje = "Ingrese un NIT válido";
                                }
                            }

                            if (validaCorreo)
                            {
                                var userMail = db.Usuario.Where(u => u.correo == model.email).FirstOrDefault();

                                if (userMail != null)
                                {
                                    guardar = false;
                                    mensaje = "Este correo ya está siendo utilizado";
                                }
                            }

                            if (guardar)
                            {
                                user.correo = model.email;
                                user.nombre = model.nombre;
                                user.apellido = model.apellido;
                                user.fechaNacimiento = model.fechaNacimiento;
                                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                                tipo = 1;
                                mensaje = "Información actualizada correctamente";
                                Session["usuario"] = user;
                                correo = user.correo;
                            }
                        }
                    }
                    else
                    {
                        mensaje = "Asegurese de modificar al menos un registro";
                    }
                }
            }
            catch (Exception ex)
            {
                mensaje = "Error al cambiar contraseña";
            }

            return Json(new { tipo = tipo, mensaje = mensaje, correo = correo }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Cambia la contraseña del usuario
        /// </summary>
        /// <returns>Json con tipo y mensaje</returns>
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult ChangePassword(ChangePasswordModel model)
        {
            string mensaje = "";
            int tipo = 0;

            if (!ModelState.IsValid)
            {
                //Extrae el primer mensaje de error que tenga el modelo
                mensaje = ModelState.Values.Select(e => e.Errors).Where(e => e.Count > 0).FirstOrDefault().Select(v => v.ErrorMessage).FirstOrDefault();
            }
            else
            {
                using (var db = new VentaArticulosCreditoEntities())
                {
                    var passEncrypt = Encrypt.Encripta(model.passwordActual);
                    var user = (Usuario)Session["usuario"];
                    var passUser = db.Usuario.Where(u => u.codigo == user.codigo && u.password == passEncrypt).FirstOrDefault();

                    if(passUser != null)
                    {
                        try
                        {
                            passUser.password = Encrypt.Encripta(model.newPassword);
                            db.Entry(passUser).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            tipo = 1;
                            mensaje = "Se ha cambiado su contraseña correctamente";
                        }
                        catch (Exception ex)
                        {
                            mensaje = "Error al cambiar contraseña";
                        }
                    }
                    else
                    {
                        mensaje = "Contraseña incorrecta";
                    }
                }
            }

            return Json(new { tipo = tipo, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Devuelve los municipios del departamento seleccionado
        /// </summary>
        /// <param name="departamento"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetMunicipios(int departamento)
        {
            int tipo = 0;
            string mensaje = "";
            SelectList listaMunicipios = null;

            using (var db = new VentaArticulosCreditoEntities())
            {
                try
                {
                    listaMunicipios = new SelectList(db.Municipio.Where(m => m.codigoDepartamento == departamento).ToList(), "codigo", "nombre");
                    tipo = 1;
                }catch(Exception ex)
                {
                    mensaje = "Error al recuperar municipios";
                }
            }

            return Json(new { tipo = tipo, mensaje = mensaje, listaMunicipios = listaMunicipios }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Crea la dirección del usuario
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult CreateDirection(CreateDirectionModel model)
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
                using (var db = new VentaArticulosCreditoEntities())
                {
                    try
                    {
                        var user = (Usuario)Session["usuario"];
                        var direccion = new Direccion_Usuario(user.codigo, Convert.ToInt32(model.telefono), model.direccion, model.municipio, model.nombre, model.apellido);

                        db.Direccion_Usuario.Add(direccion);
                        db.SaveChanges();
                        tipo = 1;
                        mensaje = "Dirección agregada correctamente";
                    }catch(Exception ex)
                    {
                        mensaje = "Error al crear dirección";
                    }

                }
            }

            return Json(new { tipo = tipo, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Devuelve la dirección a editar
        /// </summary>
        /// <param name="direccion"></param>
        /// <returns></returns>
        public ActionResult EditDirectionPartialView(int direccion)
        {
            PerfilPageModel model = new PerfilPageModel();

            using (var db = new VentaArticulosCreditoEntities())
            {
                model.direccion = db.Direccion_Usuario.Include("Municipio.Departamento").Where(d => d.codigo == direccion).FirstOrDefault();
                ViewBag.ListaDepartamentos = db.Departamento.ToList();
                ViewBag.ListaMunicipios = db.Municipio.Where(m => m.codigoDepartamento == model.direccion.Municipio.Departamento.codigo).ToList();
            }

            return PartialView("EditDirectionPartialView", model);
        }

        /// <summary>
        /// Edita una dirección
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult EditDirection(EditDirectionModel model)
        {
            int tipo = 0;
            string mensaje = "";

            try
            {
                if (!ModelState.IsValid)
                {
                    //Extrae el primer mensaje de error que tenga el modelo
                    mensaje = ModelState.Values.Select(e => e.Errors).Where(e => e.Count > 0).FirstOrDefault().Select(v => v.ErrorMessage).FirstOrDefault();
                }
                else
                {
                    using (var db = new VentaArticulosCreditoEntities())
                    {
                        var user = (Usuario)Session["usuario"];
                        var oldDirec = db.Direccion_Usuario.Include("Municipio.Departamento").Where(d => d.codigo == model.codigo).FirstOrDefault();
                        
                        if (HuboCambios(oldDirec, model))
                        {
                            oldDirec.nombre = model.nombre;
                            oldDirec.apellido = model.apellido;
                            oldDirec.telefono = Convert.ToInt32(model.telefono);
                            oldDirec.direccion = model.direccion;
                            oldDirec.codigoMunicipio = model.municipio;

                            db.Entry(oldDirec).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            tipo = 1;
                            mensaje = "Dirección modificada correctamente";
                        }
                        else
                        {
                            mensaje = "Asegurese de modificar al menos un registro";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                mensaje = "Error al cambiar contraseña";
            }

            return Json(new { tipo = tipo, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Elimina la dirección seleccionada
        /// </summary>
        /// <param name="direccion">Dirección</param>
        /// <returns><Json/returns>
        [HttpPost]
        public JsonResult DeleteDirection(int direccion)
        {
            int tipo = 0;
            string mensaje = "";

            using (var db = new VentaArticulosCreditoEntities())
            {
                try
                {
                    var direct = db.Direccion_Usuario.Where(d => d.codigo == direccion).FirstOrDefault();

                    db.Direccion_Usuario.Remove(direct);
                    db.SaveChanges();
                    tipo = 1;
                    mensaje = "Dirección eliminada correctamente";
                }catch(Exception ex)
                {
                    mensaje = "Error al eliminar dirección";
                }
            }

            return Json(new { tipo = tipo, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
    }
}
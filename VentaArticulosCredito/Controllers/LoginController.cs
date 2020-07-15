using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using VentaArticulosCredito.DBEntities;
using VentaArticulosCredito.Helpers;
using VentaArticulosCredito.Models.Login;

namespace VentaArticulosCredito.Controllers
{
    public class LoginController : ControlController
    {
        /// <summary>
        /// Método que devuelve la vista del login
        /// </summary>
        /// <returns>Vista del login</returns>
        [HttpGet]
        public ActionResult Login()
        {
            if (!ValidaUsuario()) return View();

            return RedirectToAction("Inicio","Inicio");
        }

        /// <summary>
        /// Valida las credenciales del usuario
        /// </summary>
        /// <param name="model">Modelo para el login</param>
        /// <returns>Json con tipo y mensaje</returns>
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Login(LoginViewModel model)
        {
            int tipo = 0;
            string mensaje = "";
            string correo = "";

            if (!ModelState.IsValid)
            {
                //Extrae el primer mensaje de error que tenga el modelo
                mensaje = ModelState.Values.Select(e => e.Errors).Where(e => e.Count > 0).FirstOrDefault().Select(v => v.ErrorMessage).FirstOrDefault();
            }
            else
            {
                using (var db = new VentaArticulosCreditoEntities())
                {
                    var userMail = db.Usuario.Where(u => u.correo == model.email).FirstOrDefault();

                    if (userMail != null)
                    {
                        if (userMail.estado == 1)
                        {
                            var passwordEncrypt = Encrypt.Encripta(model.password);

                            var userCredentials = db.Usuario.Where(u => u.correo == model.email && u.password == passwordEncrypt).FirstOrDefault();

                            if (userCredentials != null)
                            {
                                tipo = 1;
                                correo = userCredentials.correo;
                                Session["usuario"] = userCredentials;
                                Session["rolUsuario"] = userCredentials.codigoRol;
                            }
                            else
                            {
                                mensaje = "Contraseña incorrecta";
                            }
                        }
                        else
                        {
                            tipo = 2;
                            mensaje = "Aún no ha compleado su registro";
                        }
                    }
                    else
                    {
                        mensaje = "Correo no registrado";
                    }
                }
            }

            return Json(new { tipo = tipo, mensaje = mensaje, correo = correo }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Registra un nuevo cliente en la plataforma
        /// </summary>
        /// <param name="model">Modelo de registro</param>
        /// <returns>Json con mensaje y tipo de mensaje</returns>
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Register(RegisterViewModel model)
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
                        var userMail = db.Usuario.Where(u => u.correo == model.email).FirstOrDefault();
                        var userNIT = db.Usuario.Where(u => u.nit == model.nit).FirstOrDefault();

                        if (userMail != null)
                        {
                            mensaje = "Este correo ya está registrado";
                        }
                        else if (userNIT != null)
                        {
                            mensaje = "Este nit ya está siendo utilizado";
                        }
                        else
                        {
                            if (ValidarNIT.ValidaNIT(model.nit))
                            {
                                var newUser = new Usuario(model.email, Encrypt.Encripta(model.password), model.nombre, model.apellido, model.fechaNacimiento, model.nit, 1, 0);

                                db.Usuario.Add(newUser);
                                db.SaveChanges();
                                MailController().SendRegisterMail(model, ref mensaje, ref tipo);
                            }
                            else
                            {
                                mensaje = "Ingrese un nit válido";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        mensaje = "Error al registrarse";
                    }
                }
            }

            return Json(new { tipo = tipo, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Completa el registro de un usuario nuevo, es la confirmación
        /// </summary>
        /// <param name="email">Correo</param>
        /// <returns>Json con tipo de mensaje y mensaje</returns>
        [HttpPost]
        public JsonResult CompleteRegister(string email)
        {
            int tipo = 0;
            string mensaje = "";

            using (var db = new VentaArticulosCreditoEntities())
            {
                var user = db.Usuario.Where(u => u.correo == email).FirstOrDefault();

                if (user != null) {
                    if (user.estado == 0)
                    {
                        try
                        {
                            user.estado = 1;
                            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            tipo = 1;
                            mensaje = "Registro completado exitosamente";
                        }
                        catch (Exception ex)
                        {
                            mensaje = "Error al completar registro";
                        }
                    }
                    else
                    {
                        tipo = 2;
                    }
                }
                else
                {
                    mensaje = "El registro no puede completarse";
                }
            }

                return Json(new { tipo = tipo, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Envia el correo para cambiar la contraseña por olvido
        /// </summary>
        /// <param name="model">Modelo de la vista</param>
        /// <returns>Json con tipo y mensaje</returns>
        public JsonResult SendForgotPassword(ChangePasswordModel model)
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
                    var user = db.Usuario.Where(u => u.correo == model.email).FirstOrDefault();

                    if(user != null)
                    {
                        MailController().SendResetPassword(user, ref mensaje, ref tipo);
                    }
                    else
                    {
                        mensaje = "Este correo no está registrado";
                    }
                }
            }

            return Json(new { tipo = tipo, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retorna la vista de reset password
        /// </summary>
        /// <returns>Vista</returns>
        [HttpGet]
        public ActionResult ResetPassword(string token, string correo)
        {
            if (Token.LeerToken(correo, token))
            {
                return View();
            }

            return RedirectToAction("Login", "Login");
        }

        /// <summary>
        /// Restaura la contraseña del usuario
        /// </summary>
        /// <returns>Json con tipo y mensaje</returns>
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult ResetPassword(ResetPasswordViewModel model)
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
                    var user = db.Usuario.Where(u => u.correo == model.correo).FirstOrDefault();

                    if (user != null)
                    {
                        try
                        {
                            user.password = Encrypt.Encripta(model.password);
                            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            Helpers.Token.ModificarToken(model.correo, model.token);
                            tipo = 1;
                            mensaje = "Se ha restaurado su contraseña correctamente";
                        }
                        catch (Exception ex)
                        {
                            mensaje = "Error al restaurar contraseña";
                        }
                    }
                    else
                    {
                        mensaje = "El usuario ya no existe";
                    }
                }
            }

            return Json(new { tipo = tipo, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Recarga la sesión del usuario que está logueado
        /// </summary>
        /// <param name="correo">Correo del usuario</param>
        /// <returns>Json con tipo</returns>
        [HttpPost]
        public JsonResult ReloadSession(string correo)
        {
            int tipo = 0;

            using (var db = new VentaArticulosCreditoEntities())
            {
                var user = db.Usuario.Where(u => u.correo == correo).FirstOrDefault();
                
                if(user != null)
                {
                    tipo = 1;
                    Session["usuario"] = user;
                    Session["rolUsuario"] = user.codigoRol;
                }
            }

            return Json(new { tipo = tipo }, JsonRequestBehavior.AllowGet);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using VentaArticulosCredito.DBEntities;
using VentaArticulosCredito.Helpers;
using VentaArticulosCredito.Models.Login;

namespace VentaArticulosCredito.Controllers
{
    public class MailController : ControlController
    {
        /// <summary>
        /// Envía el correo de registro
        /// </summary>
        /// <param name="correo">Correo del usuario registrado</param>
        /// <returns>True o false si fue enviado o no</returns>
        public Boolean SendRegisterMail(RegisterViewModel model, ref string mensaje, ref int tipo)
        {
            Boolean enviado = false;
            string content = "";
            MailMessage mail = new MailMessage();

            try
            {
                content = System.IO.File.ReadAllText(RegisterTemplate());
                content = content.Replace("$$$_nombreUsuario", model.nombre + " " + model.apellido);
                content = content.Replace("$$$_link", URL_Register + "?email=" + model.email);
                mail.From = new MailAddress("cyber.shop.register.gt@gmail.com", "Cyber Shop");
                mail.To.Add(new MailAddress(model.email, model.nombre + " " + model.apellido));
                mail.Subject = "Registro Cyber Shop";
                mail.IsBodyHtml = true;
                mail.Body = content;

                ClienteSmtp().Send(mail);
                tipo = 1;
                mensaje = "Le hemos enviado un email para completar el registro";
            }
            catch (Exception ex)
            {
                mensaje = "Error al enviar correo";
            }

            return enviado;
        }

        /// <summary>
        /// Envía el correo con el link para restaurar la contraseña
        /// </summary>
        /// <param name="usuario">Usuario al que debe enviarse el correo</param>
        public void SendResetPassword(Usuario usuario, ref string mensaje, ref int tipo)
        {
            if (!Helpers.Token.LeerToken(usuario.correo))
            {
                string content = "";
                Guid token = Guid.NewGuid();
                MailMessage mail = new MailMessage();

                try
                {
                    content = System.IO.File.ReadAllText(ResetPasswordTemplate());
                    content = content.Replace("$$$_nombreUsuario", usuario.nombre + " " + usuario.apellido);
                    content = content.Replace("$$$_link", URL_Reset + token.ToString().Replace("-", "_") + "&correo=" + usuario.correo);
                    mail.From = new MailAddress("cyber.shop.register.gt@gmail.com", "Cyber Shop");
                    mail.To.Add(new MailAddress(usuario.correo, usuario.nombre + " " + usuario.apellido));
                    mail.Subject = "Restaurar Contraseña";
                    mail.IsBodyHtml = true;
                    mail.Body = content;

                    ClienteSmtp().Send(mail);
                    Token.GuardarToken(usuario.correo, token.ToString().Replace("-", "_"));
                    tipo = 1;
                    mensaje = "Le hemos enviado un email para restaurar su contraseña";
                }
                catch (Exception ex)
                {
                    mensaje = "Error al enviar correo";
                }
            }
            else
            {
                tipo = 2;
                mensaje = "Ya le hemos enviado un correo";
            }
        }

        /// <summary>
        /// Envía correo al confirmar orden
        /// </summary>
        /// <param name="usuario">Usuario</param>
        /// <param name="tipo">Tipo mensaje</param>
        /// <param name="mensaje">Mensaje</param>
        public void SendConfirmOrder(Usuario usuario, ref int tipo, ref string mensaje, int orden)
        {
            string content = "";
            MailMessage mail = new MailMessage();

            try
            {
                content = System.IO.File.ReadAllText(ConfirmOrderTemplate());
                content = content.Replace("$$$_nombreUsuario", usuario.nombre + " " + usuario.apellido);
                content = content.Replace("$$$_ordenCompra", orden.ToString());
                content = content.Replace("$$$_link", URL_ConfirmOrder + orden.ToString());
                mail.From = new MailAddress("cyber.shop.register.gt@gmail.com", "Cyber Shop");
                mail.To.Add(new MailAddress(usuario.correo, usuario.nombre + " " + usuario.apellido));
                mail.Subject = "Confirmación Orden";
                mail.IsBodyHtml = true;
                mail.Body = content;

                ClienteSmtp().Send(mail);
                tipo = 1;
                mensaje = "Le hemos enviado un email con la información de su orden";
            }
            catch (Exception ex)
            {
                mensaje = "Error al enviar correo";
            }
        }

        /// <summary>
        /// Envía correo al cancelar orden
        /// </summary>
        /// <param name="usuario">Usuario</param>
        /// <param name="tipo">Tipo mensaje</param>
        /// <param name="mensaje">Mensaje</param>
        public void SendCancelOrder(Usuario usuario, ref int tipo, ref string mensaje, int orden)
        {
            string content = "";
            MailMessage mail = new MailMessage();

            try
            {
                content = System.IO.File.ReadAllText(CancelOrderTemplate());
                content = content.Replace("$$$_nombreUsuario", usuario.nombre + " " + usuario.apellido);
                content = content.Replace("$$$_ordenCompra", orden.ToString());
                content = content.Replace("$$$_link", URL_ConfirmOrder + orden.ToString());
                mail.From = new MailAddress("cyber.shop.register.gt@gmail.com", "Cyber Shop");
                mail.To.Add(new MailAddress(usuario.correo, usuario.nombre + " " + usuario.apellido));
                mail.Subject = "Cancelación Orden";
                mail.IsBodyHtml = true;
                mail.Body = content;

                ClienteSmtp().Send(mail);
                tipo = 1;
                mensaje = "Le hemos enviado un email con la información de su orden";
            }
            catch (Exception ex)
            {
                mensaje = "Error al enviar correo";
            }
        }
    }
}
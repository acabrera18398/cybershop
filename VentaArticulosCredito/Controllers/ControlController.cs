using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using VentaArticulosCredito.DBEntities;
using VentaArticulosCredito.Models.Perfil;

namespace VentaArticulosCredito.Controllers
{
    public class ControlController : Controller
    {
        /// <summary>
        /// Constante de la ruta para registrarse
        /// </summary>
        public const string URL_Register = "http://localhost:54016/Login/Login";

        /// <summary>
        /// Constante de la ruta para restaurar contraseña
        /// </summary>
        public const string URL_Reset = "http://localhost:54016/Login/ResetPassword?token=";

        /// <summary>
        /// Constante de la ruta para ver confirmación de la orden
        /// </summary>
        public const string URL_ConfirmOrder = "http://localhost:54016/Compras/Compra?orden=";

        /// <summary>
        /// Devuelve el cliente SMTP para los correos
        /// </summary>
        /// <returns></returns>
        public SmtpClient ClienteSmtp()
        {
            SmtpClient cliente = new SmtpClient("smtp.gmail.com", 587);
            cliente.Credentials = new NetworkCredential("cyber.shop.register.gt@gmail.com", "CyberShop_GT");
            cliente.EnableSsl = true;

            return (cliente);
        }

        /// <summary>
        /// Devuelve la ruta del template para el registro
        /// </summary>
        /// <returns></returns>
        public string RegisterTemplate()
        {
            return (AppDomain.CurrentDomain.BaseDirectory + @"Content\Templates\registerMail.html");
        }

        /// <summary>
        /// Devuelve la ruta del template para restuarar la contraseña
        /// </summary>
        /// <returns></returns>
        public string ResetPasswordTemplate()
        {
            return (AppDomain.CurrentDomain.BaseDirectory + @"Content\Templates\resetPassword.html");
        }

        /// <summary>
        /// Devuelve la ruta del template para de la confirmación de la orden
        /// </summary>
        /// <returns></returns>
        public string ConfirmOrderTemplate()
        {
            return (AppDomain.CurrentDomain.BaseDirectory + @"Content\Templates\confirmedOrder.html");
        }

        /// <summary>
        /// Devuelve la ruta del template para de la cancelación de la orden
        /// </summary>
        /// <returns></returns>
        public string CancelOrderTemplate()
        {
            return (AppDomain.CurrentDomain.BaseDirectory + @"Content\Templates\cancelOrder.html");
        }

        /// <summary>
        /// Devuelve la ruta de las imágenes
        /// </summary>
        /// <returns></returns>
        public string ImagesPath()
        {
            return (AppDomain.CurrentDomain.BaseDirectory + @"Content\Images\");
        }

        /// <summary>
        /// Retorna un objeto MailControler
        /// </summary>
        public MailController MailController()
        {
            return (new MailController());
        }

        /// <summary>
        /// Valida si hay un usuario logueado
        /// </summary>
        /// <returns>True o false si hay usuario</returns>
        public Boolean ValidaUsuario()
        {
            if (Session["usuario"] != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Valida si hubo cambios en los datos del usuario
        /// </summary>
        /// <param name="oldUser"></param>
        /// <param name="newUser"></param>
        /// <returns></returns>
        public Boolean HuboCambios(Usuario oldUser, UpdateInfoModel newUser, ref Boolean validaNIT, ref Boolean validaCorreo)
        {
            Boolean hubo = false;

            if (newUser.email != oldUser.correo || newUser.nombre != oldUser.nombre || newUser.apellido != oldUser.apellido
                || newUser.fechaNacimiento != oldUser.fechaNacimiento || newUser.nit != oldUser.nit)
                hubo = true;

            if (newUser.nit != oldUser.nit)
                validaNIT = true;

            if (newUser.email != oldUser.correo)
                validaCorreo = true;

            return hubo;
        }

        /// <summary>
        /// Valida si hubo cambios en los datos de la dirección
        /// </summary>
        /// <param name="oldUser"></param>
        /// <param name="newUser"></param>
        /// <returns></returns>
        public Boolean HuboCambios(Direccion_Usuario oldDirec, EditDirectionModel newDirec)
        {

            if (newDirec.nombre != oldDirec.nombre || newDirec.apellido != oldDirec.apellido || Convert.ToInt32(newDirec.telefono) != oldDirec.telefono
                || newDirec.direccion != oldDirec.direccion || newDirec.municipio != oldDirec.codigoMunicipio)
                return true;

            return false;
        }
    }
}
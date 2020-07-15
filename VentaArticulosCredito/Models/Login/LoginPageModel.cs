using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VentaArticulosCredito.Models.Login
{
    public class LoginPageModel
    {
        /// <summary>
        /// Modelo del formulario del login
        /// </summary>
        public LoginViewModel loginModel { get; set; }

        /// <summary>
        /// Modelo del formulario del registro
        /// </summary>
        public RegisterViewModel registerModel { get; set; }

        /// <summary>
        /// Modelo del formulario para reiniciar contraseña
        /// </summary>
        public ChangePasswordModel resetPasswordModel { get; set; }
    }
}
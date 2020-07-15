using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VentaArticulosCredito.Models.Login
{
    public class ResetPasswordViewModel
    {
        /// <summary>
        /// Nueva contraseña
        /// </summary>
        [Required(ErrorMessage = "Ingrese contraseña")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$", ErrorMessage = "Ingrese una contraseña más segura")]
        public string password { get; set; }

        /// <summary>
        /// Confirmación nueva contraseña
        /// </summary>
        [Required(ErrorMessage = "Ingrese confirmación de contraseña")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Las contraseñas no coinciden")]
        public string confirmPassword { get; set; }

        /// <summary>
        /// Token a validar
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// Correo del usuario
        /// </summary>
        public string correo { get; set; }
    }
}
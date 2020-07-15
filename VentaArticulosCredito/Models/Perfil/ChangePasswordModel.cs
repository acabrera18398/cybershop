using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VentaArticulosCredito.Models.Perfil
{
    public class ChangePasswordModel
    {
        /// <summary>
        /// Contraseña
        /// </summary>
        [Required(ErrorMessage = "Ingrese contraseña")]
        [DataType(DataType.Password)]
        public string passwordActual { get; set; }

        /// <summary>
        /// Nueva contraseña
        /// </summary>
        [Required(ErrorMessage = "Ingrese contraseña")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$", ErrorMessage = "Ingrese una contraseña más segura")]
        public string newPassword { get; set; }

        /// <summary>
        /// Confirmación nueva contraseña
        /// </summary>
        [Required(ErrorMessage = "Ingrese confirmación de contraseña")]
        [DataType(DataType.Password)]
        [Compare("newPassword", ErrorMessage = "Las contraseñas no coinciden")]
        public string confirmPassword { get; set; }
    }
}
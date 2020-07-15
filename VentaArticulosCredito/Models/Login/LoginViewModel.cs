using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VentaArticulosCredito.Models.Login
{
    public class LoginViewModel
    {
        /// <summary>
        /// Correo para iniciar sesión
        /// </summary>
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Ingrese correo")]
        [RegularExpression(@"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$", ErrorMessage = "Ingrese un correo válido")]
        public string email { get; set; }

        /// <summary>
        /// Contraseña dle usuario
        /// </summary>
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Ingrese contraseña")]
        public string password { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VentaArticulosCredito.Models.Login
{
    public class ChangePasswordModel
    {
        /// <summary>
        /// Correo al que se enviará la solicitud de reinicio de contraseña
        /// </summary>
        [Required(ErrorMessage = "Ingrese su correo")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$", ErrorMessage = "Ingrese un correo válido")]
        public string email { get; set; }
    }
}
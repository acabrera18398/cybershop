using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VentaArticulosCredito.Models.Perfil
{
    public class UpdateInfoModel
    {
        /// <summary>
        /// Correo de registro nuevo
        /// </summary>
        [Required(ErrorMessage = "Ingrese correo")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$", ErrorMessage = "Ingrese un correo válido")]
        public string email { get; set; }

        /// <summary>
        /// Nombre del nuevo usuario
        /// </summary>
        [Required(ErrorMessage = "Ingrese nombre")]
        [DataType(DataType.Text)]
        public string nombre { get; set; }

        /// <summary>
        /// Apellido del nuevo usuario
        /// </summary>
        [Required(ErrorMessage = "Ingrese apellido")]
        [DataType(DataType.Text)]
        public string apellido { get; set; }

        /// <summary>
        /// Fecha nacimiento del nuevo usuario
        /// </summary>
        [Required(ErrorMessage = "Ingrese fecha de nacimiento")]
        [DataType(DataType.Text)]
        public DateTime fechaNacimiento { get; set; }

        /// <summary>
        /// NIT del nuevo usuario
        /// </summary>
        [Required(ErrorMessage = "Ingrese NIT")]
        [DataType(DataType.Text)]
        public string nit { get; set; }
    }
}
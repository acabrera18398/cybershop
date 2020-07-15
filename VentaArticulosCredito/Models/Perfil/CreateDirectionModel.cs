using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VentaArticulosCredito.Models.Perfil
{
    public class CreateDirectionModel
    {
        /// <summary>
        /// Nombre 
        /// </summary>
        [Required(ErrorMessage = "Ingrese nombre")]
        [DataType(DataType.Text)]
        public string nombre { get; set; }

        /// <summary>
        /// Apellido
        /// </summary>
        [Required(ErrorMessage = "Ingrese apellido")]
        [DataType(DataType.Text)]
        public string apellido { get; set; }

        /// <summary>
        /// Dirección
        /// </summary>
        [Required(ErrorMessage = "Ingrese dirección")]
        [DataType(DataType.Text)]
        public string direccion { get; set; }

        /// <summary>
        /// Teléfono
        /// </summary>
        [Required(ErrorMessage = "Ingrese teléfono")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^[0-9]{8}$", ErrorMessage = "Ingrese un número de teléfono válido")]
        public string telefono { get; set; }

        /// <summary>
        /// Municipio
        /// </summary>
        [Required(ErrorMessage = "Seleccione un departamento")]
        [DataType(DataType.Text)]
        public int departamento { get; set; }

        /// <summary>
        /// Municipio
        /// </summary>
        [Required(ErrorMessage = "Seleccione un municipio")]
        [DataType(DataType.Text)]
        public int municipio { get; set; }
    }
}
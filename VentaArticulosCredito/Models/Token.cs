using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VentaArticulosCredito.Models
{
    public class Token
    {
        /// <summary>
        /// Constructor con parámetros
        /// </summary>
        public Token(string correo, string token, int estado, DateTime fechaEmision, DateTime fechaExpiracion)
        {
            this.correo = correo;
            this.token = token;
            this.estado = estado;
            this.fechaEmision = fechaEmision;
            this.fechaExpiracion = fechaExpiracion;
        }

        /// <summary>
        /// Constructor vacío
        /// </summary>
        public Token()
        {

        }

        /// <summary>
        /// Correo del usuario
        /// </summary>
        public string correo { get; set; }

        /// <summary>
        /// Token del usuario
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// Estado del token
        /// </summary>
        public int estado { get; set; }

        /// <summary>
        /// Fecha en la que se emite el token
        /// </summary>
        public DateTime fechaEmision { get; set; }

        /// <summary>
        /// Fecha de vencimiento del token
        /// </summary>
        public DateTime fechaExpiracion { get; set; }
    }
}
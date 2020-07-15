using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VentaArticulosCredito.DBEntities;

namespace VentaArticulosCredito.Models.Perfil
{
    public class PerfilPageModel
    {
        /// <summary>
        /// Constructor con parámetros
        /// </summary>
        /// <param name="usuario"></param>
        public PerfilPageModel(Usuario usuario)
        {
            this.usuario = usuario;
        }

        /// <summary>
        /// Constructor sin parámetros
        /// </summary>
        /// <param name="usuario"></param>
        public PerfilPageModel()
        {
            
        }

        /// <summary>
        /// Usuario logueado
        /// </summary>
        public Usuario usuario { get; set; }

        /// <summary>
        /// Modelo de vista para actualizar información
        /// </summary>
        public UpdateInfoModel updateModel { get; set; }

        /// <summary>
        /// Modelo de vista para cambiar contraseña
        /// </summary>
        public ChangePasswordModel changeModel { get; set; }

        /// <summary>
        /// Modelo de vista para crear dirección
        /// </summary>
        public CreateDirectionModel directionModel { get; set; }

        /// <summary>
        /// Dirección a editar
        /// </summary>
        public Direccion_Usuario direccion { get; set; }
    }
}
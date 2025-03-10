//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VentaArticulosCredito.DBEntities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Usuario()
        {
            this.Carrito = new HashSet<Carrito>();
            this.Direccion_Usuario = new HashSet<Direccion_Usuario>();
        }

        public Usuario(string correo, string password, string nombre, string apellido, DateTime fechaNacimiento, string nit, int codigoRol, int estado)
        {
            this.correo = correo;
            this.password = password;
            this.nombre = nombre;
            this.apellido = apellido;
            this.fechaNacimiento = fechaNacimiento;
            this.nit = nit;
            this.codigoRol = codigoRol;
            this.estado = estado;
        }

        public int codigo { get; set; }
        public string correo { get; set; }
        public string password { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public Nullable<System.DateTime> fechaNacimiento { get; set; }
        public string nit { get; set; }
        public Nullable<int> codigoRol { get; set; }
        public Nullable<int> estado { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Carrito> Carrito { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Direccion_Usuario> Direccion_Usuario { get; set; }
        public virtual Rol Rol { get; set; }
    }
}

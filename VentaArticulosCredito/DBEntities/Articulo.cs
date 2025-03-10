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
    
    public partial class Articulo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Articulo()
        {
            this.DetalleFactura = new HashSet<DetalleFactura>();
            this.Imagen_Articulo = new HashSet<Imagen_Articulo>();
            this.Inventario = new HashSet<Inventario>();
        }
        
        public Articulo(string descripcion, int codigoSubCategoria, string nombre)
        {
            this.descripcion = descripcion;
            this.codigoSubCategoria = codigoSubCategoria;
            this.nombre = nombre;
        }

        public int codigo { get; set; }
        public string descripcion { get; set; }
        public Nullable<int> codigoSubCategoria { get; set; }
        public string nombre { get; set; }
    
        public virtual SubCategoria SubCategoria { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleFactura> DetalleFactura { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Imagen_Articulo> Imagen_Articulo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inventario> Inventario { get; set; }
    }
}

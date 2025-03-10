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
    
    public partial class Inventario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Inventario()
        {
            this.Carrito = new HashSet<Carrito>();
            this.Descuento_Articulo = new HashSet<Descuento_Articulo>();
        }
        
        public Inventario(int codigoArticulo, double precioCompra, double precioVenta, int cantidad, DateTime fechaCompra)
        {
            this.codigoArticulo = codigoArticulo;
            this.precioCompra = (decimal)precioCompra;
            this.precioVenta = (decimal)precioVenta;
            this.cantidad = cantidad;
            this.fechaCompra = fechaCompra;
        }

        public int correlativo { get; set; }
        public Nullable<int> codigoArticulo { get; set; }
        public Nullable<decimal> precioCompra { get; set; }
        public Nullable<decimal> precioVenta { get; set; }
        public Nullable<int> cantidad { get; set; }
        public Nullable<System.DateTime> fechaCompra { get; set; }
    
        public virtual Articulo Articulo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Carrito> Carrito { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Descuento_Articulo> Descuento_Articulo { get; set; }
    }
}

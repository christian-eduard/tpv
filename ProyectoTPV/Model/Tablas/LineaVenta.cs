using System.ComponentModel.DataAnnotations.Schema;


namespace ProyectoTPV.Model
{
    [Table("LineaVentas")]
    [PropertyChanged.ImplementPropertyChanged]
    public class LineaVenta : PropertyValidateModel
    {
        public int LineaVentaId { get; set; }
        public int Unidades { get; set; }
        //public int? ProductoID { get; set; }
        public int VarianteProductoId { get; set; }
        public int TicketVentaId { get; set; }

        //PROPIEDADES DE NAVEGACION

        public virtual TicketVenta TicketVenta { get; set; }
        //public virtual Producto Producto { get; set; }
        public virtual VarianteProducto VarianteProducto { get; set; }
    }
}

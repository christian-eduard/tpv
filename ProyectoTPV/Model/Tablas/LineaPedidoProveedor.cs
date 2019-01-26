using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTPV.Model
{
    [AddINotifyPropertyChangedInterface]
    [Table("LineaPedidoProveedores")]
    public class LineaPedidoProveedor : PropertyValidateModel
    {
        public int LineaPedidoProveedorId { get; set; }
        public int Cantidad { get; set; }


        [StringLength(255, MinimumLength = 0)]
        public string Comentario { get; set; }
        //propiedades de navegacion
        public virtual VarianteProducto VarianteProducto { get; set; }
        public virtual PedidoProveedor PedidoProveedor { get; set; }
    }
}

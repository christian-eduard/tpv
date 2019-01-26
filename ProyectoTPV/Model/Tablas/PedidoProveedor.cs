using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTPV.Model
{
    [Table("PedidoProveedores")]
    public class PedidoProveedor : PropertyValidateModel
    {
        public PedidoProveedor()
        {
            LineaPedidoProveedor = new HashSet<LineaPedidoProveedor>();
        }
        public int PedidoProveedorId { get; set; }
        public DateTime FechaPedido { get; set; }
        public bool Recibido { get; set; }

        //Propiedad de navegacion
        public virtual Proveedor Proveedor { get; set; }
        public virtual ICollection<LineaPedidoProveedor> LineaPedidoProveedor { get; set; }
    }
}
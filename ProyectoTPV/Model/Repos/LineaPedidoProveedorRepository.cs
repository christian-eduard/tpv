using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTPV.Model
{
    public class LineaPedidoProveedorRepository :GenericRepository<LineaPedidoProveedor>
    {
        public LineaPedidoProveedorRepository(TpvEntities context) : base(context)
        {
        }
    }
}

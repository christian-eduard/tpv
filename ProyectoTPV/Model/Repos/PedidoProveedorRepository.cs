using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTPV.Model
{
    public class PedidoProveedorRepository:GenericRepository<PedidoProveedor>
    {
        public PedidoProveedorRepository(TpvEntities context) : base(context)
        {
        }
    }
}

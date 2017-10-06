using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTPV.Model
{
    public class TicketVentaRepository : GenericRepository<TicketVenta>
    {
        public TicketVentaRepository(TpvEntities context) : base(context)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTPV.Model
{
    public class LineaVentaRepository : GenericRepository<LineaVenta>
    {
        public LineaVentaRepository(TpvEntities context) : base(context)
        {
        }
    }
}

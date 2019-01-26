using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTPV.Model
{
   public class CajaRepository : GenericRepository<Caja>
    {
        public CajaRepository(TpvEntities context) : base(context)
        {
        }
    }
}

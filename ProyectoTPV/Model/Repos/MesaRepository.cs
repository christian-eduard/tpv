using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTPV.Model
{
    public class MesaRepository : GenericRepository<Mesa>
    {
        public MesaRepository(TpvEntities context) : base(context)
        {
        }
    }
}

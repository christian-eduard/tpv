using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTPV.Model
{
    public class ReservaRepository : GenericRepository<Reserva>
    {
        public ReservaRepository(TpvEntities context) : base(context)
        {
        }
    }
}

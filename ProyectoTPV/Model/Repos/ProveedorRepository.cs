using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTPV.Model
{
    public class ProveedorRepository : GenericRepository<Proveedor>
    {
        public ProveedorRepository(TpvEntities context) : base(context)
        {
        }
    }
}

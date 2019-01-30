using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTPV.Model
{
    public class VendedorRepository : GenericRepository<Usuario>
    {
        public VendedorRepository(PosEntities context) : base(context)
        {
        }
    }
}

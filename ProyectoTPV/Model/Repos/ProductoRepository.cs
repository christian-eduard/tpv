using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTPV.Model
{
    public class ProductoRepository : GenericRepository<Producto>
    {
        public ProductoRepository(TpvEntities context) : base(context)
        {
        }
    }
}

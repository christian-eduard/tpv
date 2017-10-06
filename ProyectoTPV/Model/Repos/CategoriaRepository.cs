using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTPV.Model
{
    public class CategoriaRepository : GenericRepository<Categoria>
    {
        public CategoriaRepository(TpvEntities context) : base(context)
        {
        }
    }
}

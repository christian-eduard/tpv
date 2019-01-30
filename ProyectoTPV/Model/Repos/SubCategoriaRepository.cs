using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTPV.Model
{
    public class SubCategoriaRepository : GenericRepository<SubCategoria>
    {
        public SubCategoriaRepository(PosEntities context) : base(context)
        {

        }
    }
}

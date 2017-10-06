using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTPV.Model
{
    public class UsuarioRepository : GenericRepository<Usuario>
    {
        public UsuarioRepository(TpvEntities context) : base(context)
        {
        }
    }
}

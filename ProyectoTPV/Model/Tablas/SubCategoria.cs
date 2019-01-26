using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTPV.Model
{
    public class SubCategoria
    {
        public SubCategoria()
        {
            Producto = new HashSet<Producto>();
        }
        public int SubCategoriaId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string RutaImagen { get; set; }

        //Propiedades de nevegacion
        public virtual Categoria Categoria { get; set; }
        public virtual ICollection<Producto> Producto { get; set; }
    }
}

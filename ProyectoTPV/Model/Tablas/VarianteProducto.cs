using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTPV.Model
{
    public class VarianteProducto
    {
        public int VarianteProductoId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string RutaImagen { get; set; }
        public int Stock { get; set; }
        public decimal Precio { get; set; }

        //Propiedad de navegacion
        public virtual Producto Producto { get; set; }
    }
}

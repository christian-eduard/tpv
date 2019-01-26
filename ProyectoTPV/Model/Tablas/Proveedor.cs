using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTPV.Model
{
    [Table("Proveedores")]
    public class Proveedor
    {
        public Proveedor()
        {
            Producto = new HashSet<Producto>();
        }
        public int ProveedorId { get; set; }
        public string Nombre { get; set; }
        public string NombreContacto { get; set; }
        public string Cif { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Producto> Producto { get; set; }

    }
}

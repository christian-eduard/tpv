using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTPV.Model
{
    [ImplementPropertyChanged]
    public class Categoria : PropertyValidateModel
    {
        public Categoria()
        {
            Producto = new HashSet<Producto>();
        }

        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "Nombre obligatorio")]
        [StringLength(100, MinimumLength = 2)]
        [Index(IsUnique = true)]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }
        public string RutaImagen { get; set; }

        //PROPIEDADES DE NAVEGACION

        public virtual ICollection<Producto> Producto { get; set; }
    }
}

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
    [Table("Categorias")]
    [AddINotifyPropertyChangedInterface]
    public class Categoria : PropertyValidateModel
    {
        public Categoria()
        {
            SubCategoria = new HashSet<SubCategoria>();
        }

        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "Nombre obligatorio")]
        [StringLength(100, MinimumLength = 2)]
        [Index(IsUnique = true)]
        public string Nombre { get; set; }

        [StringLength(255, MinimumLength = 0)]
        public string Descripcion { get; set; }

        [StringLength(255, MinimumLength = 0)]
        public string RutaImagen { get; set; }

        //PROPIEDADES DE NAVEGACION
        public virtual ICollection<SubCategoria> SubCategoria { get; set; }
    }
}

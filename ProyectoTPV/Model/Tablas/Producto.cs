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
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Producto : PropertyValidateModel
    {
        public Producto()
        {
            LineaVenta = new HashSet<LineaVenta>();
        }

        public int ProductoId { get; set; }


        [Required(ErrorMessage = "Nombre obligatorio")]
        [StringLength(100, MinimumLength = 2)]
        [Index(IsUnique = true)]
        public string Nombre { get; set; }


        [Required(ErrorMessage = "Introduce un stock inicial")]
        [Range(0, 9999)]
        public int Stock { get; set; }

        [Required]
        [RegularExpression(@"^\d+(,\d{1,2})?$",ErrorMessage ="Formato del precio incorrecto")]
        public decimal Precio { get; set; }


        public int Iva { get; set; }


        public string Descripcion { get; set; }


        public string RutaImagen { get; set; }


        //PROPIEDADES DE NAVEGACION
        [Required (ErrorMessage ="Elige una categoría")]
        public virtual Categoria Categoria { get; set; }

        public virtual ICollection<LineaVenta> LineaVenta { get; set; }
    }
}

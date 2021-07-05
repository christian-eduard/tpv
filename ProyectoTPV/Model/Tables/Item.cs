using PropertyChanged;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace OpenPOS.Model
{
    [AddINotifyPropertyChangedInterface]
    public class Item : PropertyValidateModel
    {
        public Item()
        {
            SalesLine = new HashSet<SalesLine>();
        }

        public int ItemId { get; set; }


        [Required(ErrorMessage = "Nombre obligatorio")]
        [StringLength(100, MinimumLength = 2)]
        [Index(IsUnique = true)]
        public string Name { get; set; }


        [Required(ErrorMessage = "Introduce un stock inicial")]
        [Range(0, 9999)]
        public int Stock { get; set; }

        [Required]
        [RegularExpression(@"^\d+(,\d{1,2})?$",ErrorMessage ="Formato del precio incorrecto")]
        public decimal Price { get; set; }


        public int Tax { get; set; }

        [StringLength(100, MinimumLength = 0)]
        public string Description { get; set; }

        [StringLength(100, MinimumLength = 0)]
        public string ImagePath { get; set; }


        //PROPIEDADES DE NAVEGACION
        [Required (ErrorMessage ="Elige una categoría")]
        public virtual Group Group { get; set; }

        public virtual ICollection<SalesLine> SalesLine { get; set; }
    }
}

using PropertyChanged;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenPOS.Model
{
    [AddINotifyPropertyChangedInterface]
    public class Group : PropertyValidateModel
    {
        public Group()
        {
            Item = new HashSet<Item>();
        }

        public int GroupId { get; set; }

        [Required(ErrorMessage = "Nombre obligatorio")]
        [StringLength(100, MinimumLength = 2)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        [StringLength(100, MinimumLength = 0)]
        public string Description { get; set; }

        [StringLength(100, MinimumLength = 0)]
        public string ImagePath { get; set; }

        //PROPIEDADES DE NAVEGACION

        public virtual ICollection<Item> Item { get; set; }
    }
}

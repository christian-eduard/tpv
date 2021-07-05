using PropertyChanged;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenPOS.Model
{
    [AddINotifyPropertyChangedInterface]
    public class User : PropertyValidateModel
    {
        public User()
        {
            SalesLine = new HashSet<Invoice>();
        }

        public int UserId { get; set; }

        [Required(ErrorMessage = "Nombre obligatorio")]
        [StringLength(40, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(40, MinimumLength = 0)]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Login obligatorio")]
        [Index(IsUnique = true)]
        [StringLength(15, MinimumLength = 2)]
        public string Login { get; set; }

        [Required(ErrorMessage = "Pin obligatorio")]
        [RegularExpression(@"\d\d\d\d", ErrorMessage = "introduce un pin de 4 cifras")]
        [StringLength(100, MinimumLength = 2)]
        public string Pin { get; set; }

        [Required(ErrorMessage = "tipo de usuario obligatorio")]
        [RegularExpression("usuario|admin")]
        [StringLength(100, MinimumLength = 2)]
        public string UserType { get; set; }

        [RegularExpression(@"(^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$)?",ErrorMessage ="Introduce un correo válido")]
        [StringLength(100, MinimumLength = 2)]
        public string Email { get; set; }

        [StringLength(100, MinimumLength = 2)]
        public string ImagePath { get; set; }

        //PROPIEDADES DE NAVEGACION

        public virtual ICollection<Invoice> SalesLine { get; set; }
    }
}


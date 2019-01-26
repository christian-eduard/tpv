using PropertyChanged;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoTPV.Model
{
    [Table("Clientes")]
    [AddINotifyPropertyChangedInterface]
    public class Cliente : PropertyValidateModel
    {
        public Cliente()
        {
            TicketVenta = new HashSet<TicketVenta>();
        }

        public int ClienteId { get; set; }
        public int Puntos { get; set; }

        [StringLength(255, MinimumLength = 0)]
        public string Nombre { get; set; }

        [StringLength(255, MinimumLength = 0)]
        public string Apellidos { get; set; }

        [StringLength(255, MinimumLength = 0)]
        public string Direccion { get; set; }

        [StringLength(255, MinimumLength = 0)]
        public string Dni { get; set; }

        [StringLength(255, MinimumLength = 0)]
        public string Telefono { get; set; }

        [StringLength(255, MinimumLength = 0)]
        public string Email { get; set; }

        //Propiedad de navegacion
        public virtual ICollection<TicketVenta> TicketVenta { get; set; }

    }
}

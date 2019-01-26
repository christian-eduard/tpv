using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTPV.Model
{
    [Table("Mesas")]
    public class Mesa : PropertyValidateModel
    {
        public Mesa()
        {
            Reserva = new HashSet<Reserva>();
            TicketVenta = new HashSet<TicketVenta>();
        }

        public int MesaId { get; set; }
        public string NombreMesa { get; set; }
        public int Capacidad { get; set; }
        public decimal IncrementoMesa { get; set; }

        //propiedad de navegacion

        public virtual ICollection<TicketVenta> TicketVenta { get; set; }

        public virtual ICollection<Reserva> Reserva { get; set; }
    }
}

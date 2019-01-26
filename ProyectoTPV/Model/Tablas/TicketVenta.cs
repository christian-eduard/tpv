using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTPV.Model
{
    [Table("TicketsVentas")]
    [PropertyChanged.ImplementPropertyChanged]
    public class TicketVenta : PropertyValidateModel
    {
        public TicketVenta()
        {
            LineaVenta = new HashSet<LineaVenta>();
        }

        public int TicketVentaId { get; set; }

        public DateTime FechaHora { get; set; }


        //PROPIEDAD DE NAVEGACION

        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<LineaVenta> LineaVenta { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Mesa Mesa { get; set; }
    }
}

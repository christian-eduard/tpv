using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTPV.Model
{
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
    }
}

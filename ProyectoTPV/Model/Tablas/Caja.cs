using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTPV.Model
{
    [Table("Cajas")]
    [AddINotifyPropertyChangedInterface]
    public class Caja : PropertyValidateModel
    {
        public Caja()
        {
            TicketVenta = new HashSet<TicketVenta>();


        }
        public int CajaId { get; set; }
        public decimal DineroInicial { get; set; }
        public decimal DineroFinal { get; set; }
        public DateTime FechaHoraApertura { get; set; }
        public DateTime? FechaHoraRecuento { get; set; }
        //Propiedad de navegacion
        public virtual ICollection<TicketVenta> TicketVenta { get; set; }
        public virtual Usuario Vendedor { get; set; }
    }

}

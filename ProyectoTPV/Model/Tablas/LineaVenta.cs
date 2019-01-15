using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTPV.Model
{
    [AddINotifyPropertyChangedInterface]
    public class LineaVenta : PropertyValidateModel
    {
        public int LineaVentaId { get; set; }
        public int Unidades { get; set; }
        public int ProductoID { get; set; }
        public int TicketVentaId { get; set; }

        //PROPIEDADES DE NAVEGACION

        public virtual TicketVenta TicketVenta { get; set; }
        public virtual Producto Producto { get; set; }
    }
}

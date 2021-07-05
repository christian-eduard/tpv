using System;
using System.Collections.Generic;

namespace OpenPOS.Model
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Invoice : PropertyValidateModel
    {
        public Invoice()
        {
            SalesLine = new HashSet<SalesLine>();
        }

        public int InvoiceId { get; set; }

        public DateTime Timestamp { get; set; }


        //PROPIEDAD DE NAVEGACION

        public virtual User User { get; set; }
        public virtual ICollection<SalesLine> SalesLine { get; set; }
    }
}

using PropertyChanged;

namespace OpenPOS.Model
{
    [AddINotifyPropertyChangedInterface]
    public class SalesLine : PropertyValidateModel
    {
        public int SalesLineId { get; set; }
        public int Unit { get; set; }
        public int ItemId { get; set; }
        public int InvoiceId { get; set; }

        //PROPIEDADES DE NAVEGACION

        public virtual Invoice Invoice { get; set; }
        public virtual Item Item { get; set; }
    }
}

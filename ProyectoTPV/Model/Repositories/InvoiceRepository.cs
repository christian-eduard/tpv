namespace OpenPOS.Model
{
    public class InvoiceRepository : GenericRepository<Invoice>
    {
        public InvoiceRepository(OpenPOSEntities context) : base(context)
        {
        }
    }
}

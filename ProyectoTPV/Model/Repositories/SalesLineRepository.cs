namespace OpenPOS.Model
{
    public class SalesLineRepository : GenericRepository<SalesLine>
    {
        public SalesLineRepository(OpenPOSEntities context) : base(context)
        {
        }
    }
}

namespace OpenPOS.Model
{
    public class ItemRepository : GenericRepository<Item>
    {
        public ItemRepository(OpenPOSEntities context) : base(context)
        {
        }
    }
}

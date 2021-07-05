namespace OpenPOS.Model
{
    public class GroupRepository : GenericRepository<Group>
    {
        public GroupRepository(OpenPOSEntities context) : base(context)
        {
        }
    }
}

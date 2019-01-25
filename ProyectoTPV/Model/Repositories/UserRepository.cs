namespace OpenPOS.Model
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(OpenPOSEntities context) : base(context)
        {
        }
    }
}

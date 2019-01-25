using System.Data.Entity;
using MySql.Data.Entity;


namespace OpenPOS.Model
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class OpenPOSEntities : DbContext
    {
        public DbSet<Group> Group { get; set; }
        public DbSet<SalesLine> SalesLine { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<User> User { get; set; }

        public OpenPOSEntities() : base("openPOS")
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
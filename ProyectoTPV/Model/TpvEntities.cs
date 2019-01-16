using System.Data.Entity;
using MySql.Data.Entity;


namespace ProyectoTPV.Model
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class TpvEntities : DbContext
    {
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<LineaVenta> LineaVenta { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<TicketVenta> TicketVenta { get; set; }
        public DbSet<Usuario> Usuario { get; set; }

        public TpvEntities() : base("tpv")
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ProyectoTPV.Model
{
    public class TpvEntities : DbContext
    {

        public TpvEntities()
                 : base("TpvEntities")
        { }

        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<LineaVenta> LineaVenta { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<TicketVenta> TicketVenta { get; set; }
        public DbSet<Usuario> Usuario { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
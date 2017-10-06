namespace ProyectoTPV.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Model;

    internal sealed class Configuration : DbMigrationsConfiguration<ProyectoTPV.Model.TpvEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ProyectoTPV.Model.TpvEntities context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Usuario.AddOrUpdate(
              new Usuario
              {
                  UsuarioId = 1,
                  Nombre = "Administrador",
                  Apellidos = "",
                  Login = "Admin",
                  Pin = "0000",
                  RutaImagen = "admin.jpg",
                  TipoUsuario = "admin"
              }
            );
            context.SaveChanges();
            context.Categoria.AddOrUpdate(
            new Categoria
            {
                CategoriaId = 1,
                Nombre = "Varios",
                Descripcion = "Productos sin catalogar"
            }
            );

            context.SaveChanges();

        }
    }
}

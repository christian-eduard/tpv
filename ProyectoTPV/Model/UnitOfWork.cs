using ProyectoTPV.Model;
using System;
using System.Linq;


namespace ProyectoTPV.Model
{
    public class UnitOfWork : IDisposable
    {
        private TpvEntities context = new TpvEntities();
        private bool disposed = false;


        private CategoriaRepository categoriaRepository;
        private LineaVentaRepository lineaVentaRepository;
        private ProductoRepository productoRepository;
        private TicketVentaRepository ticketVentaRepository;
        private UsuarioRepository usuarioRepository;

        public CategoriaRepository CategoriaRepository
        {
            get
            {
                if (this.categoriaRepository == null)
                {
                    this.categoriaRepository =
                        new CategoriaRepository(context);
                }

                return categoriaRepository;
            }
        }
        public LineaVentaRepository LineaVentaRepository
        {
            get
            {
                if (this.lineaVentaRepository == null)
                {
                    this.lineaVentaRepository =
                        new LineaVentaRepository(context);
                }

                return lineaVentaRepository;
            }
        }
        public ProductoRepository ProductoRepository
        {
            get
            {
                if (this.productoRepository == null)
                {
                    this.productoRepository =
                        new ProductoRepository(context);
                }

                return productoRepository;
            }
        }
        public TicketVentaRepository TicketVentaRepository
        {
            get
            {
                if (this.ticketVentaRepository == null)
                {
                    this.ticketVentaRepository =
                        new TicketVentaRepository(context);
                }

                return ticketVentaRepository;
            }
        }
        public UsuarioRepository UsuarioRepository
        {
            get
            {
                if (this.usuarioRepository == null)
                {
                    this.usuarioRepository =
                        new UsuarioRepository(context);
                }

                return usuarioRepository;
            }
        }


        public void Save()
        {
            context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
using ProyectoTPV.Model;
using System;
using System.Linq;


namespace ProyectoTPV.Model
{
    public class UnitOfWork : IDisposable
    {
        private PosEntities context = new PosEntities();
        private bool disposed = false;


        private CategoriaRepository categoriaRepository;
        private LineaVentaRepository lineaVentaRepository;
        private ProductoRepository productoRepository;
        private TicketVentaRepository ticketVentaRepository;
        private VendedorRepository vendedorRepository;
        private CajaRepository cajaRepository;
        private ClienteRepository clienteRepository;
        private LineaPedidoProveedorRepository lineaPedidoProveedorRepository;
        private PedidoProveedorRepository pedidoProveedorRepository;
        private ProveedorRepository proveedorRepository;
        private ReservaRepository reservaRepository;
        private SubCategoriaRepository subCategoriaRepository;
        private VarianteProductoRepository varianteProductoRepository;
        private MesaRepository mesaRepository;

        public MesaRepository MesaRepository
        {
            get
            {
                if (this.mesaRepository == null)
                {
                    this.mesaRepository = new MesaRepository(context);
                }
                return mesaRepository;
            }
        }


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
        public VendedorRepository VendedorRepository
        {
            get
            {
                if (this.vendedorRepository == null)
                {
                    this.vendedorRepository =
                        new VendedorRepository(context);
                }

                return vendedorRepository;
            }
        }
        public CajaRepository CajaRepository
        {
            get
            {
                if (this.cajaRepository == null)
                {
                    this.cajaRepository =
                        new CajaRepository(context);
                }

                return cajaRepository;
            }
        }
        public ClienteRepository ClienteRepository
        {
            get
            {
                if (this.clienteRepository == null)
                {
                    this.clienteRepository =
                        new ClienteRepository(context);
                }

                return clienteRepository;
            }
        }
        public LineaPedidoProveedorRepository LineaPedidoProveedorRepository
        {
            get
            {
                if (this.lineaPedidoProveedorRepository == null)
                {
                    this.lineaPedidoProveedorRepository =
                        new LineaPedidoProveedorRepository(context);
                }

                return lineaPedidoProveedorRepository;
            }
        }
        public PedidoProveedorRepository PedidoProveedorRepository
        {
            get
            {
                if (this.pedidoProveedorRepository == null)
                {
                    this.pedidoProveedorRepository =
                        new PedidoProveedorRepository(context);
                }

                return pedidoProveedorRepository;
            }
        }
        public ProveedorRepository ProveedorRepository
        {
            get
            {
                if (this.proveedorRepository == null)
                {
                    this.proveedorRepository =
                        new ProveedorRepository(context);
                }

                return proveedorRepository;
            }
        }
        public ReservaRepository ReservaRepository
        {
            get
            {
                if (this.reservaRepository == null)
                {
                    this.reservaRepository =
                        new ReservaRepository(context);
                }

                return reservaRepository;
            }
        }
        public SubCategoriaRepository SubCategoriaRepository
        {
            get
            {
                if (this.subCategoriaRepository == null)
                {
                    this.subCategoriaRepository =
                        new SubCategoriaRepository(context);
                }

                return subCategoriaRepository;
            }
        }
        public VarianteProductoRepository VarianteProductoRepository
        {
            get
            {
                if (this.varianteProductoRepository == null)
                {
                    this.varianteProductoRepository =
                        new VarianteProductoRepository(context);
                }

                return varianteProductoRepository;
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
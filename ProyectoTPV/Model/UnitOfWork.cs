using System;

namespace OpenPOS.Model
{
    public class UnitOfWork : IDisposable
    {
        private OpenPOSEntities context = new OpenPOSEntities();
        private bool disposed = false;


        private GroupRepository groupRepository;
        private SalesLineRepository salesLineRepository;
        private ItemRepository itemRepository;
        private InvoiceRepository invoiceRepository;
        private UserRepository userRepository;

        public GroupRepository GroupRepository
        {
            get
            {
                if (this.groupRepository == null)
                {
                    this.groupRepository =
                        new GroupRepository(context);
                }

                return groupRepository;
            }
        }
        public SalesLineRepository SalesLineRepository
        {
            get
            {
                if (this.salesLineRepository == null)
                {
                    this.salesLineRepository =
                        new SalesLineRepository(context);
                }

                return salesLineRepository;
            }
        }
        public ItemRepository ItemRepository
        {
            get
            {
                if (this.itemRepository == null)
                {
                    this.itemRepository =
                        new ItemRepository(context);
                }

                return itemRepository;
            }
        }
        public InvoiceRepository InvoiceRepository
        {
            get
            {
                if (this.invoiceRepository == null)
                {
                    this.invoiceRepository =
                        new InvoiceRepository(context);
                }

                return invoiceRepository;
            }
        }
        public UserRepository UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository =
                        new UserRepository(context);
                }

                return userRepository;
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
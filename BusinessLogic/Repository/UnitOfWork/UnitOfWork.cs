using BusinessLogic.Entity;
using System.Data.Entity;

namespace BusinessLogic.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TestInvoiceContext _dbContext;

        #region Repositories   
        public IRepository<Customer> CustomerRepository => new GenericRepository<Customer>(_dbContext);

        public IRepository<CustomerType> CustomerTypeRepository => new GenericRepository<CustomerType>(_dbContext);

        public IRepository<Invoice> InvoiceRepository => new GenericRepository<Invoice>(_dbContext);

        public IRepository<InvoiceDetail> InvoiceDetailRepository => new GenericRepository<InvoiceDetail>(_dbContext);
        #endregion

        public UnitOfWork(TestInvoiceContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public void RejectChanges()
        {
            foreach (var entry in _dbContext.ChangeTracker.Entries()
                         .Where(e => e.State != Microsoft.EntityFrameworkCore.EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case (Microsoft.EntityFrameworkCore.EntityState)EntityState.Added:
                        entry.State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                        break;
                    case Microsoft.EntityFrameworkCore.EntityState.Modified:
                    case Microsoft.EntityFrameworkCore.EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }
    }
}

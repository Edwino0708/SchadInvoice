
using BusinessLogic.Entity;

namespace BusinessLogic.Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<Customer> CustomerRepository { get; }
        IRepository<CustomerType> CustomerTypeRepository { get; }
        IRepository<Invoice> InvoiceRepository { get; }
        IRepository<InvoiceDetail> InvoiceDetailRepository { get; }

        /// <summary>
        /// Commits all changes
        /// </summary>
        /// 
        void Commit();
        
        /// <summary>
        /// Discards all changes that has not been commited
        /// </summary>
        void RejectChanges();
        void Dispose();
    }
}

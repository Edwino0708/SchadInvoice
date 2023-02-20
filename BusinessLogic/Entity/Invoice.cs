using System;
using System.Collections.Generic;

namespace BusinessLogic.Entity;

public partial class Invoice
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public decimal TotalItbis { get; set; }

    public decimal SubTotal { get; set; }

    public decimal Total { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; } = new List<InvoiceDetail>();
}

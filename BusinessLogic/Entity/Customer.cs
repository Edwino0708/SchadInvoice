using System;
using System.Collections.Generic;

namespace BusinessLogic.Entity;

public partial class Customer
{
    public int Id { get; set; }

    public string CustName { get; set; } = null!;

    public string Adress { get; set; } = null!;

    public bool? Status { get; set; }

    public int CustomerTypeId { get; set; }

    public virtual CustomerType CustomerType { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; } = new List<Invoice>();
}

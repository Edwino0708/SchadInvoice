using System;
using System.Collections.Generic;

namespace BusinessLogic.Entity;

public partial class CustomerType
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<Customer> Customers { get; } = new List<Customer>();
}

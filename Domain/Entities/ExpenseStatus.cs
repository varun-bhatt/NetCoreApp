using System;
using System.Collections.Generic;

namespace NetCoreApp.Domain.Entities;

public partial class ExpenseStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}

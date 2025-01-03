using System;
using System.Collections.Generic;

namespace NetCoreApp.Domain.Entities;

public partial class Expense
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; }
    public decimal? Amount { get; set; }
    public int StatusId { get; set; }
    public int ExpenseCategoryId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public virtual ExpenseCategory ExpenseCategory { get; set; } = null!;
    public virtual ExpenseStatus Status { get; set; } = null!;
    public long UserId { get; set; }
    public virtual User User { get; set; }
}

using Microsoft.EntityFrameworkCore;
using NetCoreApp.Domain.Entities;

namespace NetCoreApp.Infrastructure.Persistence
{
    public class ApplicationDbContext :DbContext
    {

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Expense> Expenses { get; set; }

        public virtual DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Expense>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.ExpenseCategory).WithMany(p => p.Expenses)
                    .HasForeignKey(d => d.ExpenseCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Expenses_Category");

                entity.HasOne(d => d.User).WithMany(p => p.Expenses)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserId");
            });

            builder.Entity<ExpenseCategory>(entity =>
            {
                entity.ToTable("ExpenseCategory");

                entity.Property(e => e.Id);
                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            builder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Name, "UK_UserName").IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);
                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(1000);
            });

            base.OnModelCreating(builder);
        }
    }
}
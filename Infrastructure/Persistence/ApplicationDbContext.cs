using Microsoft.EntityFrameworkCore;
using NetCoreApp.Domain.Entities;

namespace NetCoreApp.Infrastructure.Persistence
{
    public class ApplicationDbContext :DbContext
    {

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
            builder.Entity<Person>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.City)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.PersonId).HasColumnName("PersonID");
            });

            base.OnModelCreating(builder);
        }
    }
}
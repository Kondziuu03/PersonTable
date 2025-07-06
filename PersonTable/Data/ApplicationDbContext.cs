using Microsoft.EntityFrameworkCore;
using PersonTable.Data.Entities;

namespace PersonTable.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Email> Emails { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasMany(p => p.Emails)
                .WithOne(e => e.Person)
                .HasForeignKey(e => e.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Person>()
                .Property(p => p.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Person>()
                .Property(p => p.LastName)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Person>()
                .Property(p => p.Description)
                .HasMaxLength(500);

            modelBuilder.Entity<Email>()
                .Property(e => e.Address)
                .IsRequired();
        }
    }
}

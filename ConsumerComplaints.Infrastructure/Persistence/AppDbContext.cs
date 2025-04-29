using ConsumerComplaints.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConsumerComplaints.Infrastructure.Persistence

{

    public class AppDbContext : DbContext

    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)

        {

        }
 
        public DbSet<Complaint> Complaints { get; set; }
 
        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {

            base.OnModelCreating(modelBuilder);
 
            modelBuilder.Entity<Complaint>(entity =>

            {

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Product).IsRequired();

                entity.Property(e => e.Issue).IsRequired();

                entity.Property(e => e.Company).IsRequired();

                entity.Property(e => e.DateReceived).IsRequired();

            });

        }

    }

}
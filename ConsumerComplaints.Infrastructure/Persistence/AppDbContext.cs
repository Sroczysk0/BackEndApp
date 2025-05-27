using ConsumerComplaints.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ConsumerComplaints.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<UserEntity>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); 

            // === Complaint ===
            builder.Entity<Complaint>().ToTable("Complaint");

            builder.Entity<Complaint>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Complaint ID");
                entity.Property(e => e.SubmittedVia).HasColumnName("Submitted via");
                entity.Property(e => e.DateReceived).HasColumnName("Date received");
                entity.Property(e => e.State).HasColumnName("State");
                entity.Property(e => e.Product).HasColumnName("Product");
                entity.Property(e => e.Issue).HasColumnName("Issue");
                entity.Property(e => e.SubIssue).HasColumnName("Sub-issue");

                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // === Comment ===
            builder.Entity<Comment>().ToTable("Comments");

            builder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Complaint)
                    .WithMany()
                    .HasForeignKey(e => e.ComplaintId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .IsRequired(false);
            });

            builder.Entity<UserEntity>().OwnsOne(u => u.Details);
        }

    }
}

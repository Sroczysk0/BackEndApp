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
            base.OnModelCreating(builder); // nic nie usuwać!

            // complaints – ważne: z małej litery, jak w SQLite!
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
            });

            // comments – z relacją do Complaint i User
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

            // seed admin
            var adminId = "c0aec95b-0600-4023-a98f-85dbb42b727f";
            var adminCreatedAt = "2025-04-08 00:00:00";

            var adminUser = new UserEntity
            {
                Id = adminId,
                Email = "admin@wsei.edu.pl",
                NormalizedEmail = "ADMIN@WSEI.EDU.PL",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                ConcurrencyStamp = adminId,
                SecurityStamp = adminId,
                PasswordHash = "AQAAAAIAAYagAAAAEMtbq6wc6wWCS4vx0zLMtIFdVX3b0gTxXfXDKgJ6EH6aD5fJ8egCbq+wo6SY5i6LYQ=="
            };

            builder.Entity<UserEntity>().HasData(adminUser);

            builder.Entity<UserEntity>()
                .OwnsOne(u => u.Details)
                .HasData(new
                {
                    UserEntityId = adminId,
                    FirstName = "Admin",
                    LastName = "Root",
                    PhoneNumber = "+48123123123",
                    DateOfBirth = "1990-01-01",
                    Country = "Poland",
                    CreatedAt = adminCreatedAt
                });
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF;

public class AppDbContext:IdentityDbContext<UserEntity>
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected AppDbContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); //nic nie usuwac!!!
        var adminId = "c0aec95b-0600-4023-a98f-85dbb42b727f";
        var adminCreatedAt = new DateTime(2025, 04, 8);
        var adminUser = new UserEntity()
        {
            Id = adminId,
            Email = "admin@wsei.edu.pl",
            NormalizedEmail = "ADMIN@WSEI.EDU.Pl",
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            ConcurrencyStamp = adminId,
            SecurityStamp = adminId,
        };

        //PasswordHasher<UserEntity> ph = new PasswordHasher<UserEntity>();
        //var hash = ph.HashPassword(adminUser, "1234!");
        adminUser.PasswordHash = "AQAAAAIAAYagAAAAEMtbq6wc6wWCS4vx0zLMtIFdVX3b0gTxXfXDKgJ6EH6aD5fJ8egCbq+wo6SY5i6LYQ==";
        //Console.WriteLine(hash);

        builder.Entity<UserEntity>().HasData(adminUser);
        
        builder.Entity<UserEntity>()
            .OwnsOne(u => u.Details)
            .HasData(
                new
                {
                    UserEntityId = adminId,
                    CreatedAt = adminCreatedAt,
                }
            );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source = c:\\data\\users.db");
    }
}
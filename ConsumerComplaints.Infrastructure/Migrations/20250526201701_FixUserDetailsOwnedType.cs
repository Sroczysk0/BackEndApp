using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsumerComplaints.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixUserDetailsOwnedType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c0aec95b-0600-4023-a98f-85dbb42b727f");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "Details_Country", "Details_CreatedAt", "Details_DateOfBirth", "Details_FirstName", "Details_LastName", "Details_PhoneNumber" },
                values: new object[] { "c0aec95b-0600-4023-a98f-85dbb42b727f", 0, "c0aec95b-0600-4023-a98f-85dbb42b727f", "admin@wsei.edu.pl", false, false, null, "ADMIN@WSEI.EDU.PL", "ADMIN", "AQAAAAIAAYagAAAAEFHYOyNVRUbU+fYw9MOK0JQZGH7xSff+JxJVRux+S4R2ZQ03x2MzM9MJdIb97rLVEQ==", null, false, "c0aec95b-0600-4023-a98f-85dbb42b727f", false, "admin", "Poland", "2025-04-08 00:00:00", "1990-01-01", "Admin", "Root", "+48123123123" });
        }
    }
}

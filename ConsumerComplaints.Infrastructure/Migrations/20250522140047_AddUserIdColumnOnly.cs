using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsumerComplaints.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdColumnOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Complaint",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Complaint");
        }
    }
}
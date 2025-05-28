using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsumerComplaints.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToComplaint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c0aec95b-0600-4023-a98f-85dbb42b727f",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFHYOyNVRUbU+fYw9MOK0JQZGH7xSff+JxJVRux+S4R2ZQ03x2MzM9MJdIb97rLVEQ==");

            // 🟥 USUŃ/zakomentuj tę część:
            // migrationBuilder.AddForeignKey(
            //     name: "FK_Complaint_AspNetUsers_UserId",
            //     table: "Complaint",
            //     column: "UserId",
            //     principalTable: "AspNetUsers",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Restrict);
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 🟥 NIE PRÓBUJEMY USUWAĆ FK, bo nie był dodany
            // migrationBuilder.DropForeignKey(...);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c0aec95b-0600-4023-a98f-85dbb42b727f",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMtbq6wc6wWCS4vx0zLMtIFdVX3b0gTxXfXDKgJ6EH6aD5fJ8egCbq+wo6SY5i6LYQ==");
        }

    }
}

 using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    public partial class son : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Doctors_DoctorID",
                table: "Randevular");

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_PoliclinicID",
                table: "Randevular",
                column: "PoliclinicID");

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Doctors_DoctorID",
                table: "Randevular",
                column: "DoctorID",
                principalTable: "Doctors",
                principalColumn: "DoctorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Policlinics_PoliclinicID",
                table: "Randevular",
                column: "PoliclinicID",
                principalTable: "Policlinics",
                principalColumn: "PoliclinicID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Doctors_DoctorID",
                table: "Randevular");

            migrationBuilder.DropForeignKey(
                name: "FK_Randevular_Policlinics_PoliclinicID",
                table: "Randevular");

            migrationBuilder.DropIndex(
                name: "IX_Randevular_PoliclinicID",
                table: "Randevular");

            migrationBuilder.AddForeignKey(
                name: "FK_Randevular_Doctors_DoctorID",
                table: "Randevular",
                column: "DoctorID",
                principalTable: "Doctors",
                principalColumn: "DoctorID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

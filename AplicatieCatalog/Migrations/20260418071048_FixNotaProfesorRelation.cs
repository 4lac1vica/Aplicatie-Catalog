using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AplicatieCatalog.Migrations
{
    /// <inheritdoc />
    public partial class FixNotaProfesorRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Materii_MaterieId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Students_StudentId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Profesori_AspNetUsers_ApplicationUserId",
                table: "Profesori");

            migrationBuilder.AddColumn<int>(
                name: "ProfesorId",
                table: "Grades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Grades_ProfesorId",
                table: "Grades",
                column: "ProfesorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Materii_MaterieId",
                table: "Grades",
                column: "MaterieId",
                principalTable: "Materii",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Profesori_ProfesorId",
                table: "Grades",
                column: "ProfesorId",
                principalTable: "Profesori",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Students_StudentId",
                table: "Grades",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Profesori_AspNetUsers_ApplicationUserId",
                table: "Profesori",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Materii_MaterieId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Profesori_ProfesorId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Students_StudentId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Profesori_AspNetUsers_ApplicationUserId",
                table: "Profesori");

            migrationBuilder.DropIndex(
                name: "IX_Grades_ProfesorId",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "ProfesorId",
                table: "Grades");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Materii_MaterieId",
                table: "Grades",
                column: "MaterieId",
                principalTable: "Materii",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Students_StudentId",
                table: "Grades",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Profesori_AspNetUsers_ApplicationUserId",
                table: "Profesori",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

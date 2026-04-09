using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RegistroEstudiantil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ajusteentidades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfesorMaterias");

            migrationBuilder.AlterColumn<string>(
                name: "Rol",
                table: "Usuarios",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "Estudiante",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true,
                oldDefaultValue: "Estudiante");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Rol",
                table: "Usuarios",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                defaultValue: "Estudiante",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValue: "Estudiante");

            migrationBuilder.CreateTable(
                name: "ProfesorMaterias",
                columns: table => new
                {
                    ProfesorId = table.Column<int>(type: "int", nullable: false),
                    MateriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfesorMaterias", x => new { x.ProfesorId, x.MateriaId });
                    table.ForeignKey(
                        name: "FK_ProfesorMaterias_Materias_MateriaId",
                        column: x => x.MateriaId,
                        principalTable: "Materias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfesorMaterias_Profesores_ProfesorId",
                        column: x => x.ProfesorId,
                        principalTable: "Profesores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ProfesorMaterias",
                columns: new[] { "MateriaId", "ProfesorId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 2 },
                    { 4, 2 },
                    { 5, 3 },
                    { 6, 3 },
                    { 7, 4 },
                    { 8, 4 },
                    { 9, 5 },
                    { 10, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfesorMaterias_MateriaId",
                table: "ProfesorMaterias",
                column: "MateriaId");
        }
    }
}


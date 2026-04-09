using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RegistroEstudiantil.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Materias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Creditos = table.Column<int>(type: "int", nullable: false, defaultValue: 3)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profesores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombres = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profesores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true, defaultValue: "Estudiante")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Grupos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Periodo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NombreGrupo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Cupo = table.Column<int>(type: "int", nullable: false, defaultValue: 40),
                    MateriaId = table.Column<int>(type: "int", nullable: false),
                    ProfesorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grupos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grupos_Materias_MateriaId",
                        column: x => x.MateriaId,
                        principalTable: "Materias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Grupos_Profesores_ProfesorId",
                        column: x => x.ProfesorId,
                        principalTable: "Profesores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "Estudiantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Documento = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Nombres = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estudiantes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Estudiantes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inscripciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstudianteId = table.Column<int>(type: "int", nullable: false),
                    GrupoClaseId = table.Column<int>(type: "int", nullable: false),
                    Periodo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscripciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inscripciones_Estudiantes_EstudianteId",
                        column: x => x.EstudianteId,
                        principalTable: "Estudiantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inscripciones_Grupos_GrupoClaseId",
                        column: x => x.GrupoClaseId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Materias",
                columns: new[] { "Id", "Codigo", "Creditos", "Nombre" },
                values: new object[,]
                {
                    { 1, "MAT101", 3, "Matemáticas I" },
                    { 2, "PRO101", 3, "Programación I" },
                    { 3, "BD101", 3, "Bases de Datos" },
                    { 4, "ING101", 3, "Inglés I" },
                    { 5, "FIS101", 3, "Física I" },
                    { 6, "EST101", 3, "Estadística" },
                    { 7, "RED101", 3, "Redes I" },
                    { 8, "SOF101", 3, "Ingeniería de Software" },
                    { 9, "SIS101", 3, "Sistemas Operativos" },
                    { 10, "ETI101", 3, "Ética Profesional" }
                });

            migrationBuilder.InsertData(
                table: "Profesores",
                columns: new[] { "Id", "Apellidos", "Nombres" },
                values: new object[,]
                {
                    { 1, "García", "Ana" },
                    { 2, "Pérez", "Luis" },
                    { 3, "López", "Marta" },
                    { 4, "Ramírez", "Carlos" },
                    { 5, "Torres", "Sofía" }
                });

            migrationBuilder.InsertData(
                table: "Grupos",
                columns: new[] { "Id", "Cupo", "MateriaId", "NombreGrupo", "Periodo", "ProfesorId" },
                values: new object[,]
                {
                    { 1, 40, 1, "A", "2025-2", 1 },
                    { 2, 40, 2, "A", "2025-2", 1 },
                    { 3, 40, 3, "A", "2025-2", 2 },
                    { 4, 40, 4, "A", "2025-2", 2 },
                    { 5, 40, 5, "A", "2025-2", 3 },
                    { 6, 40, 6, "A", "2025-2", 3 },
                    { 7, 40, 7, "A", "2025-2", 4 },
                    { 8, 40, 8, "A", "2025-2", 4 },
                    { 9, 40, 9, "A", "2025-2", 5 },
                    { 10, 40, 10, "A", "2025-2", 5 }
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
                name: "IX_Estudiantes_Documento",
                table: "Estudiantes",
                column: "Documento",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Estudiantes_UsuarioId",
                table: "Estudiantes",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Grupos_MateriaId_ProfesorId_Periodo_NombreGrupo",
                table: "Grupos",
                columns: new[] { "MateriaId", "ProfesorId", "Periodo", "NombreGrupo" },
                unique: true,
                filter: "[NombreGrupo] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Grupos_ProfesorId",
                table: "Grupos",
                column: "ProfesorId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_EstudianteId_GrupoClaseId",
                table: "Inscripciones",
                columns: new[] { "EstudianteId", "GrupoClaseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_GrupoClaseId",
                table: "Inscripciones",
                column: "GrupoClaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Materias_Codigo",
                table: "Materias",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfesorMaterias_MateriaId",
                table: "ProfesorMaterias",
                column: "MateriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inscripciones");

            migrationBuilder.DropTable(
                name: "ProfesorMaterias");

            migrationBuilder.DropTable(
                name: "Estudiantes");

            migrationBuilder.DropTable(
                name: "Grupos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Materias");

            migrationBuilder.DropTable(
                name: "Profesores");
        }
    }
}



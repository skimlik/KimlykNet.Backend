using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KimlykNet.Backend.Migrations.Data
{
    /// <inheritdoc />
    public partial class AddAppContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.CreateTable(
                name: "UserNotes",
                schema: "app",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    note_id = table.Column<Guid>(type: "uuid", nullable: false),
                    note = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2024, 7, 2, 7, 1, 40, 736, DateTimeKind.Unspecified).AddTicks(420), new TimeSpan(0, 0, 0, 0, 0))),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, defaultValue: new DateTimeOffset(new DateTime(2024, 7, 2, 7, 1, 40, 736, DateTimeKind.Unspecified).AddTicks(570), new TimeSpan(0, 0, 0, 0, 0)))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotes", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserNotes_user_id",
                schema: "app",
                table: "UserNotes",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserNotes",
                schema: "app");
        }
    }
}

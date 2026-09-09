using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyworxCredit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pengajuan_kredit",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    plafon = table.Column<decimal>(type: "numeric", nullable: false),
                    bunga = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    tenor = table.Column<int>(type: "integer", nullable: false),
                    angsuran = table.Column<decimal>(type: "numeric", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pengajuan_kredit", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_pengajuan_kredit_plafon",
                table: "pengajuan_kredit",
                column: "plafon");

            migrationBuilder.CreateIndex(
                name: "IX_pengajuan_kredit_tenor",
                table: "pengajuan_kredit",
                column: "tenor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pengajuan_kredit");
        }
    }
}

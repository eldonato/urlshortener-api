using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "urls_encurtadas",
                columns: table => new
                {
                    url_curta = table.Column<string>(
                        type: "character varying(10)",
                        maxLength: 10,
                        nullable: false
                    ),
                    url_original = table.Column<string>(type: "text", nullable: false),
                    criado_em = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    contador_cliques = table.Column<long>(type: "bigint", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_urls_encurtadas", x => x.url_curta);
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_urls_encurtadas_url_curta",
                table: "urls_encurtadas",
                column: "url_curta",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "urls_encurtadas");
        }
    }
}

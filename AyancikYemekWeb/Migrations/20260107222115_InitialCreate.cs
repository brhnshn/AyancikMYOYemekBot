using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AyancikYemekWeb.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aboneler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatId = table.Column<long>(type: "bigint", nullable: false),
                    KayitTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aboneler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "YemekMenuleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tarih = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Corba = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnaYemek = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YardimciYemek = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tatli = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MenüVarMi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YemekMenuleri", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aboneler");

            migrationBuilder.DropTable(
                name: "YemekMenuleri");
        }
    }
}

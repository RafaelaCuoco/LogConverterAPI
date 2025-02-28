using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LogConverterAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogsOriginais",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Conteudo = table.Column<string>(maxLength: 2147483647, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogsOriginais", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogsTransformados",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LogOriginalId = table.Column<int>(nullable: false),
                    Conteudo = table.Column<string>(maxLength: 2147483647, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogsTransformados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogsTransformados_LogsOriginais_LogOriginalId",
                        column: x => x.LogOriginalId,
                        principalTable: "LogsOriginais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogsOriginais_Id",
                table: "LogsOriginais",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LogsTransformados_LogOriginalId",
                table: "LogsTransformados",
                column: "LogOriginalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogsTransformados");

            migrationBuilder.DropTable(
                name: "LogsOriginais");
        }
    }
}

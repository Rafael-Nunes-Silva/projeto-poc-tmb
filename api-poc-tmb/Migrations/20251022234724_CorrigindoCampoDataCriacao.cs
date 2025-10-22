using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_poc_tmb.Migrations
{
    /// <inheritdoc />
    public partial class CorrigindoCampoDataCriacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Sata_criacao",
                table: "orders",
                newName: "Data_criacao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Data_criacao",
                table: "orders",
                newName: "Sata_criacao");
        }
    }
}

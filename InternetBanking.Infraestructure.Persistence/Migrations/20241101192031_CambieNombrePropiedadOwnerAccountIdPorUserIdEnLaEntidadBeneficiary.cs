using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternetBanking.Infraestructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CambieNombrePropiedadOwnerAccountIdPorUserIdEnLaEntidadBeneficiary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OwnerAccountId",
                table: "Beneficiaries",
                newName: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Beneficiaries",
                newName: "OwnerAccountId");
        }
    }
}

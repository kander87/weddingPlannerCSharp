using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeddingPlanner.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Association_Users_UserId",
                table: "Association");

            migrationBuilder.DropForeignKey(
                name: "FK_Association_Weddings_WeddingId",
                table: "Association");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Association",
                table: "Association");

            migrationBuilder.RenameTable(
                name: "Association",
                newName: "Associations");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Weddings",
                newName: "Address");

            migrationBuilder.RenameIndex(
                name: "IX_Association_WeddingId",
                table: "Associations",
                newName: "IX_Associations_WeddingId");

            migrationBuilder.RenameIndex(
                name: "IX_Association_UserId",
                table: "Associations",
                newName: "IX_Associations_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Associations",
                table: "Associations",
                column: "AssociationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Associations_Users_UserId",
                table: "Associations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Associations_Weddings_WeddingId",
                table: "Associations",
                column: "WeddingId",
                principalTable: "Weddings",
                principalColumn: "WeddingId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Associations_Users_UserId",
                table: "Associations");

            migrationBuilder.DropForeignKey(
                name: "FK_Associations_Weddings_WeddingId",
                table: "Associations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Associations",
                table: "Associations");

            migrationBuilder.RenameTable(
                name: "Associations",
                newName: "Association");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Weddings",
                newName: "Email");

            migrationBuilder.RenameIndex(
                name: "IX_Associations_WeddingId",
                table: "Association",
                newName: "IX_Association_WeddingId");

            migrationBuilder.RenameIndex(
                name: "IX_Associations_UserId",
                table: "Association",
                newName: "IX_Association_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Association",
                table: "Association",
                column: "AssociationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Association_Users_UserId",
                table: "Association",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Association_Weddings_WeddingId",
                table: "Association",
                column: "WeddingId",
                principalTable: "Weddings",
                principalColumn: "WeddingId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

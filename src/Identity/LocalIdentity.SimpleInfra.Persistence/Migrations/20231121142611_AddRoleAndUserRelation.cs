using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LocalIdentity.SimpleInfra.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleAndUserRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ModifiedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedTime", "DeletedTime", "IsActive", "IsDeleted", "ModifiedTime", "Type" },
                values: new object[,]
                {
                    { new Guid("0327f1ba-81cf-478f-98d4-04fec56fc10a"), new DateTimeOffset(new DateTime(2023, 11, 21, 19, 26, 11, 127, DateTimeKind.Unspecified).AddTicks(6209), new TimeSpan(0, 5, 0, 0, 0)), null, true, false, null, 0 },
                    { new Guid("d0b0d6c0-2b7a-4b1a-9f1a-0b9b6a9a5b1a"), new DateTimeOffset(new DateTime(2023, 11, 21, 19, 26, 11, 127, DateTimeKind.Unspecified).AddTicks(6239), new TimeSpan(0, 5, 0, 0, 0)), null, true, false, null, 1 },
                    { new Guid("d0b0d6c0-2b7a-4b1a-9f1a-0b9b6a9a5b1b"), new DateTimeOffset(new DateTime(2023, 11, 21, 19, 26, 11, 127, DateTimeKind.Unspecified).AddTicks(6241), new TimeSpan(0, 5, 0, 0, 0)), null, true, false, null, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");
        }
    }
}

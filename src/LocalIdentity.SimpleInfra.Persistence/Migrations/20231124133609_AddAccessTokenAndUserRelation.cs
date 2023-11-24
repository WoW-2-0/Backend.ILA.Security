using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalIdentity.SimpleInfra.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAccessTokenAndUserRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccessTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    ExpiryTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ModifiedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0327f1ba-81cf-478f-98d4-04fec56fc10a"),
                column: "CreatedTime",
                value: new DateTimeOffset(new DateTime(2023, 11, 24, 18, 36, 8, 933, DateTimeKind.Unspecified).AddTicks(5990), new TimeSpan(0, 5, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("d0b0d6c0-2b7a-4b1a-9f1a-0b9b6a9a5b1a"),
                column: "CreatedTime",
                value: new DateTimeOffset(new DateTime(2023, 11, 24, 18, 36, 8, 933, DateTimeKind.Unspecified).AddTicks(6028), new TimeSpan(0, 5, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("d0b0d6c0-2b7a-4b1a-9f1a-0b9b6a9a5b1b"),
                column: "CreatedTime",
                value: new DateTimeOffset(new DateTime(2023, 11, 24, 18, 36, 8, 933, DateTimeKind.Unspecified).AddTicks(6031), new TimeSpan(0, 5, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_AccessTokens_UserId",
                table: "AccessTokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessTokens");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0327f1ba-81cf-478f-98d4-04fec56fc10a"),
                column: "CreatedTime",
                value: new DateTimeOffset(new DateTime(2023, 11, 24, 12, 4, 29, 898, DateTimeKind.Unspecified).AddTicks(2306), new TimeSpan(0, 5, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("d0b0d6c0-2b7a-4b1a-9f1a-0b9b6a9a5b1a"),
                column: "CreatedTime",
                value: new DateTimeOffset(new DateTime(2023, 11, 24, 12, 4, 29, 898, DateTimeKind.Unspecified).AddTicks(2336), new TimeSpan(0, 5, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("d0b0d6c0-2b7a-4b1a-9f1a-0b9b6a9a5b1b"),
                column: "CreatedTime",
                value: new DateTimeOffset(new DateTime(2023, 11, 24, 12, 4, 29, 898, DateTimeKind.Unspecified).AddTicks(2338), new TimeSpan(0, 5, 0, 0, 0)));
        }
    }
}

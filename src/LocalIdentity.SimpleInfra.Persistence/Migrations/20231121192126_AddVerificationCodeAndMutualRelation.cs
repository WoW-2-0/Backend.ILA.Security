using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalIdentity.SimpleInfra.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddVerificationCodeAndMutualRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VerificationCode",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CodeType = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ExpiryTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    VerificationLink = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationCode", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0327f1ba-81cf-478f-98d4-04fec56fc10a"),
                column: "CreatedTime",
                value: new DateTimeOffset(new DateTime(2023, 11, 22, 0, 21, 26, 31, DateTimeKind.Unspecified).AddTicks(4819), new TimeSpan(0, 5, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("d0b0d6c0-2b7a-4b1a-9f1a-0b9b6a9a5b1a"),
                column: "CreatedTime",
                value: new DateTimeOffset(new DateTime(2023, 11, 22, 0, 21, 26, 31, DateTimeKind.Unspecified).AddTicks(4848), new TimeSpan(0, 5, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("d0b0d6c0-2b7a-4b1a-9f1a-0b9b6a9a5b1b"),
                column: "CreatedTime",
                value: new DateTimeOffset(new DateTime(2023, 11, 22, 0, 21, 26, 31, DateTimeKind.Unspecified).AddTicks(4850), new TimeSpan(0, 5, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VerificationCode");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0327f1ba-81cf-478f-98d4-04fec56fc10a"),
                column: "CreatedTime",
                value: new DateTimeOffset(new DateTime(2023, 11, 21, 19, 26, 11, 127, DateTimeKind.Unspecified).AddTicks(6209), new TimeSpan(0, 5, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("d0b0d6c0-2b7a-4b1a-9f1a-0b9b6a9a5b1a"),
                column: "CreatedTime",
                value: new DateTimeOffset(new DateTime(2023, 11, 21, 19, 26, 11, 127, DateTimeKind.Unspecified).AddTicks(6239), new TimeSpan(0, 5, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("d0b0d6c0-2b7a-4b1a-9f1a-0b9b6a9a5b1b"),
                column: "CreatedTime",
                value: new DateTimeOffset(new DateTime(2023, 11, 21, 19, 26, 11, 127, DateTimeKind.Unspecified).AddTicks(6241), new TimeSpan(0, 5, 0, 0, 0)));
        }
    }
}

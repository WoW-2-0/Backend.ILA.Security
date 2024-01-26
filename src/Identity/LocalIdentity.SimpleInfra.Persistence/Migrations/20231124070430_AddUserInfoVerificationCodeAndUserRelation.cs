using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalIdentity.SimpleInfra.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserInfoVerificationCodeAndUserRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_VerificationCode_UserId",
                table: "VerificationCode",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_VerificationCode_Users_UserId",
                table: "VerificationCode",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VerificationCode_Users_UserId",
                table: "VerificationCode");

            migrationBuilder.DropIndex(
                name: "IX_VerificationCode_UserId",
                table: "VerificationCode");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0327f1ba-81cf-478f-98d4-04fec56fc10a"),
                column: "CreatedTime",
                value: new DateTimeOffset(new DateTime(2023, 11, 24, 12, 4, 8, 352, DateTimeKind.Unspecified).AddTicks(2665), new TimeSpan(0, 5, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("d0b0d6c0-2b7a-4b1a-9f1a-0b9b6a9a5b1a"),
                column: "CreatedTime",
                value: new DateTimeOffset(new DateTime(2023, 11, 24, 12, 4, 8, 352, DateTimeKind.Unspecified).AddTicks(2696), new TimeSpan(0, 5, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("d0b0d6c0-2b7a-4b1a-9f1a-0b9b6a9a5b1b"),
                column: "CreatedTime",
                value: new DateTimeOffset(new DateTime(2023, 11, 24, 12, 4, 8, 352, DateTimeKind.Unspecified).AddTicks(2697), new TimeSpan(0, 5, 0, 0, 0)));
        }
    }
}

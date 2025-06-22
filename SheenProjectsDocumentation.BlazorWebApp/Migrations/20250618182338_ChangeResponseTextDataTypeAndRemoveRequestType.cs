using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectsDocumentation.BlazorWebApp.Migrations
{
    /// <inheritdoc />
    public partial class ChangeResponseTextDataTypeAndRemoveRequestType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EndpointRequests_LookupValues_RequestTypeId",
                table: "EndpointRequests");

            migrationBuilder.DropIndex(
                name: "IX_EndpointRequests_RequestTypeId",
                table: "EndpointRequests");

            migrationBuilder.DropColumn(
                name: "RequestTypeId",
                table: "EndpointRequests");

            migrationBuilder.AlterColumn<string>(
                name: "EndpointResponseData",
                table: "EndpointResponses",
                type: "JSON",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EndpointResponseData",
                table: "EndpointResponses",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "JSON",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "RequestTypeId",
                table: "EndpointRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EndpointRequests_RequestTypeId",
                table: "EndpointRequests",
                column: "RequestTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EndpointRequests_LookupValues_RequestTypeId",
                table: "EndpointRequests",
                column: "RequestTypeId",
                principalTable: "LookupValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

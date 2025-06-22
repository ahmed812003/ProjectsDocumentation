using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectsDocumentation.BlazorWebApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabaseTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LookupTitles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookupTitles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LookupValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LookupTitleId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookupValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LookupValues_LookupTitles_LookupTitleId",
                        column: x => x.LookupTitleId,
                        principalTable: "LookupTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProjectTables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    TableName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTables_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProjectModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ModelName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModelTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectModels_LookupValues_ModelTypeId",
                        column: x => x.ModelTypeId,
                        principalTable: "LookupValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectModels_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Endpoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EndpointUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProjectModelId = table.Column<int>(type: "int", nullable: false),
                    MethodId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Endpoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Endpoints_LookupValues_MethodId",
                        column: x => x.MethodId,
                        principalTable: "LookupValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Endpoints_ProjectModels_ProjectModelId",
                        column: x => x.ProjectModelId,
                        principalTable: "ProjectModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EndpointVersions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EndpointId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Version = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndpointVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EndpointVersions_Endpoints_EndpointId",
                        column: x => x.EndpointId,
                        principalTable: "Endpoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EndpointReflections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EndpointVersionId = table.Column<int>(type: "int", nullable: false),
                    ProjectTableId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndpointReflections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EndpointReflections_EndpointVersions_EndpointVersionId",
                        column: x => x.EndpointVersionId,
                        principalTable: "EndpointVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EndpointReflections_ProjectTables_ProjectTableId",
                        column: x => x.ProjectTableId,
                        principalTable: "ProjectTables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EndpointRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EndpointVersionId = table.Column<int>(type: "int", nullable: false),
                    RequestTypeId = table.Column<int>(type: "int", nullable: false),
                    Headers = table.Column<string>(type: "JSON", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Body = table.Column<string>(type: "JSON", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    QueryParameters = table.Column<string>(type: "JSON", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndpointRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EndpointRequests_EndpointVersions_EndpointVersionId",
                        column: x => x.EndpointVersionId,
                        principalTable: "EndpointVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EndpointRequests_LookupValues_RequestTypeId",
                        column: x => x.RequestTypeId,
                        principalTable: "LookupValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EndpointResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EndpointVersionId = table.Column<int>(type: "int", nullable: false),
                    StatusCodeId = table.Column<int>(type: "int", nullable: false),
                    EndpointResponseData = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndpointResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EndpointResponses_EndpointVersions_EndpointVersionId",
                        column: x => x.EndpointVersionId,
                        principalTable: "EndpointVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EndpointResponses_LookupValues_StatusCodeId",
                        column: x => x.StatusCodeId,
                        principalTable: "LookupValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_EndpointReflections_EndpointVersionId_ProjectTableId",
                table: "EndpointReflections",
                columns: new[] { "EndpointVersionId", "ProjectTableId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EndpointReflections_ProjectTableId",
                table: "EndpointReflections",
                column: "ProjectTableId");

            migrationBuilder.CreateIndex(
                name: "IX_EndpointRequests_EndpointVersionId",
                table: "EndpointRequests",
                column: "EndpointVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_EndpointRequests_RequestTypeId",
                table: "EndpointRequests",
                column: "RequestTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EndpointResponses_EndpointVersionId",
                table: "EndpointResponses",
                column: "EndpointVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_EndpointResponses_StatusCodeId",
                table: "EndpointResponses",
                column: "StatusCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Endpoints_MethodId",
                table: "Endpoints",
                column: "MethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Endpoints_ProjectModelId_EndpointUrl_MethodId",
                table: "Endpoints",
                columns: new[] { "ProjectModelId", "EndpointUrl", "MethodId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EndpointVersions_EndpointId_Version",
                table: "EndpointVersions",
                columns: new[] { "EndpointId", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LookupTitles_Title",
                table: "LookupTitles",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LookupValues_LookupTitleId_Value",
                table: "LookupValues",
                columns: new[] { "LookupTitleId", "Value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectModels_ModelTypeId",
                table: "ProjectModels",
                column: "ModelTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectModels_ProjectId_ModelName_ModelTypeId",
                table: "ProjectModels",
                columns: new[] { "ProjectId", "ModelName", "ModelTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectName",
                table: "Projects",
                column: "ProjectName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTables_ProjectId_TableName",
                table: "ProjectTables",
                columns: new[] { "ProjectId", "TableName" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EndpointReflections");

            migrationBuilder.DropTable(
                name: "EndpointRequests");

            migrationBuilder.DropTable(
                name: "EndpointResponses");

            migrationBuilder.DropTable(
                name: "ProjectTables");

            migrationBuilder.DropTable(
                name: "EndpointVersions");

            migrationBuilder.DropTable(
                name: "Endpoints");

            migrationBuilder.DropTable(
                name: "ProjectModels");

            migrationBuilder.DropTable(
                name: "LookupValues");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "LookupTitles");
        }
    }
}

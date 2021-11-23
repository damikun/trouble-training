using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIServer.Persistence.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    ID = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ActorID = table.Column<Guid>(type: "TEXT", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EventType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WebHooks",
                columns: table => new
                {
                    ID = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WebHookUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Secret = table.Column<string>(type: "TEXT", nullable: true),
                    ContentType = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    HookEvents = table.Column<string>(type: "TEXT", nullable: true),
                    LastTrigger = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebHooks", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WebHookCreatedEvent",
                columns: table => new
                {
                    ID = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WebHookId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebHookCreatedEvent", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WebHookCreatedEvent_Events_ID",
                        column: x => x.ID,
                        principalTable: "Events",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebHookRemovedEvent",
                columns: table => new
                {
                    ID = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WebHookId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebHookRemovedEvent", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WebHookRemovedEvent_Events_ID",
                        column: x => x.ID,
                        principalTable: "Events",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebHookUpdatedEvent",
                columns: table => new
                {
                    ID = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WebHookId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebHookUpdatedEvent", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WebHookUpdatedEvent_Events_ID",
                        column: x => x.ID,
                        principalTable: "Events",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebHookHeader",
                columns: table => new
                {
                    ID = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WebHookID = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Value = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedTimestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebHookHeader", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WebHookHeader_WebHooks_WebHookID",
                        column: x => x.WebHookID,
                        principalTable: "WebHooks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebHooksHistory",
                columns: table => new
                {
                    ID = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WebHookID = table.Column<long>(type: "INTEGER", nullable: false),
                    Guid = table.Column<string>(type: "TEXT", nullable: true),
                    HookType = table.Column<int>(type: "INTEGER", nullable: false),
                    Result = table.Column<int>(type: "INTEGER", nullable: false),
                    StatusCode = table.Column<int>(type: "INTEGER", nullable: false),
                    ResponseBody = table.Column<string>(type: "TEXT", nullable: true),
                    RequestBody = table.Column<string>(type: "TEXT", nullable: true),
                    RequestHeaders = table.Column<string>(type: "TEXT", nullable: true),
                    Exception = table.Column<string>(type: "TEXT", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebHooksHistory", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WebHooksHistory_WebHooks_WebHookID",
                        column: x => x.WebHookID,
                        principalTable: "WebHooks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "WebHooks",
                columns: new[] { "ID", "ContentType", "HookEvents", "IsActive", "LastTrigger", "Secret", "WebHookUrl" },
                values: new object[] { 1L, "application/json", "hook", true, null, null, "https://localhost:5015/hookloopback" });

            migrationBuilder.InsertData(
                table: "WebHooks",
                columns: new[] { "ID", "ContentType", "HookEvents", "IsActive", "LastTrigger", "Secret", "WebHookUrl" },
                values: new object[] { 2L, "application/json", "hook", false, null, null, "https://localhost:5015/hookloopback2" });

            migrationBuilder.CreateIndex(
                name: "IX_WebHookHeader_WebHookID",
                table: "WebHookHeader",
                column: "WebHookID");

            migrationBuilder.CreateIndex(
                name: "IX_WebHooksHistory_WebHookID",
                table: "WebHooksHistory",
                column: "WebHookID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebHookCreatedEvent");

            migrationBuilder.DropTable(
                name: "WebHookHeader");

            migrationBuilder.DropTable(
                name: "WebHookRemovedEvent");

            migrationBuilder.DropTable(
                name: "WebHooksHistory");

            migrationBuilder.DropTable(
                name: "WebHookUpdatedEvent");

            migrationBuilder.DropTable(
                name: "WebHooks");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}

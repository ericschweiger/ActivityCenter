using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ActivityCenter.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserList",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserList", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "ActivityList",
                columns: table => new
                {
                    ActivityEventId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ActivityName = table.Column<string>(nullable: false),
                    ActivityStart = table.Column<DateTime>(nullable: false),
                    ActivityDuration = table.Column<int>(nullable: false),
                    ActivityDescription = table.Column<string>(nullable: false),
                    UserID = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityList", x => x.ActivityEventId);
                    table.ForeignKey(
                        name: "FK_ActivityList_UserList_UserID",
                        column: x => x.UserID,
                        principalTable: "UserList",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Joiners",
                columns: table => new
                {
                    JoinId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    ActivityEventId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Joiners", x => x.JoinId);
                    table.ForeignKey(
                        name: "FK_Joiners_ActivityList_ActivityEventId",
                        column: x => x.ActivityEventId,
                        principalTable: "ActivityList",
                        principalColumn: "ActivityEventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Joiners_UserList_UserId",
                        column: x => x.UserId,
                        principalTable: "UserList",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityList_UserID",
                table: "ActivityList",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Joiners_ActivityEventId",
                table: "Joiners",
                column: "ActivityEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Joiners_UserId",
                table: "Joiners",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Joiners");

            migrationBuilder.DropTable(
                name: "ActivityList");

            migrationBuilder.DropTable(
                name: "UserList");
        }
    }
}

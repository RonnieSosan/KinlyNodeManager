using Microsoft.EntityFrameworkCore.Migrations;

namespace KinlyNodeManager.Persistence.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Nodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Port = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Maintainer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NodeLabels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NodeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodeLabels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NodeLabels_Nodes_NodeId",
                        column: x => x.NodeId,
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "Id", "Maintainer", "Name", "Port" },
                values: new object[] { 1, "j@mail.cu", "FirstNode", "7070" });

            migrationBuilder.InsertData(
                table: "NodeLabels",
                columns: new[] { "Id", "Key", "NodeId", "Value" },
                values: new object[] { 1, "Group", 1, "API" });

            migrationBuilder.InsertData(
                table: "NodeLabels",
                columns: new[] { "Id", "Key", "NodeId", "Value" },
                values: new object[] { 2, "Type", 1, "Plartform" });

            migrationBuilder.CreateIndex(
                name: "IX_NodeLabels_NodeId",
                table: "NodeLabels",
                column: "NodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NodeLabels");

            migrationBuilder.DropTable(
                name: "Nodes");
        }
    }
}

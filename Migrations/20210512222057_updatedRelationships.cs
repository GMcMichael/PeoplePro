using Microsoft.EntityFrameworkCore.Migrations;

namespace PeoplePro.Migrations
{
    public partial class updatedRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentRoom_Department_DepartmentId",
                table: "DepartmentRoom");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentRoom_Rooms_RoomId",
                table: "DepartmentRoom");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepartmentRoom",
                table: "DepartmentRoom");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Department",
                table: "Department");

            migrationBuilder.RenameTable(
                name: "DepartmentRoom",
                newName: "DepartmentsRooms");

            migrationBuilder.RenameTable(
                name: "Department",
                newName: "Departments");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentRoom_RoomId",
                table: "DepartmentsRooms",
                newName: "IX_DepartmentsRooms_RoomId");

            migrationBuilder.AddColumn<int>(
                name: "BuildingID",
                table: "Rooms",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepartmentsRooms",
                table: "DepartmentsRooms",
                columns: new[] { "DepartmentId", "RoomId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Departments",
                table: "Departments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BuildingsDepartments",
                columns: table => new
                {
                    BuildingId = table.Column<int>(nullable: false),
                    DepartmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingsDepartments", x => new { x.BuildingId, x.DepartmentId });
                    table.ForeignKey(
                        name: "FK_BuildingsDepartments_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BuildingsDepartments_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_BuildingID",
                table: "Rooms",
                column: "BuildingID");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingsDepartments_DepartmentId",
                table: "BuildingsDepartments",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentsRooms_Departments_DepartmentId",
                table: "DepartmentsRooms",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentsRooms_Rooms_RoomId",
                table: "DepartmentsRooms",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Buildings_BuildingID",
                table: "Rooms",
                column: "BuildingID",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentsRooms_Departments_DepartmentId",
                table: "DepartmentsRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentsRooms_Rooms_RoomId",
                table: "DepartmentsRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Buildings_BuildingID",
                table: "Rooms");

            migrationBuilder.DropTable(
                name: "BuildingsDepartments");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_BuildingID",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepartmentsRooms",
                table: "DepartmentsRooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Departments",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "BuildingID",
                table: "Rooms");

            migrationBuilder.RenameTable(
                name: "DepartmentsRooms",
                newName: "DepartmentRoom");

            migrationBuilder.RenameTable(
                name: "Departments",
                newName: "Department");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentsRooms_RoomId",
                table: "DepartmentRoom",
                newName: "IX_DepartmentRoom_RoomId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepartmentRoom",
                table: "DepartmentRoom",
                columns: new[] { "DepartmentId", "RoomId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Department",
                table: "Department",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentRoom_Department_DepartmentId",
                table: "DepartmentRoom",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentRoom_Rooms_RoomId",
                table: "DepartmentRoom",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance.Migrations
{
    public partial class Teste : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    user_email = table.Column<string>(type: "nchar(100)", fixedLength: true, maxLength: 100, nullable: false),
                    user_name = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    user_passwd = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    user_dtcad = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => new { x.user_id, x.user_email });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user");
        }
    }
}

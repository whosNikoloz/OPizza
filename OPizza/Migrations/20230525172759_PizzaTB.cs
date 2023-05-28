using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPizza.Migrations
{
    /// <inheritdoc />
    public partial class PizzaTB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pizzas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PizzaName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TomatoSauce = table.Column<bool>(type: "bit", nullable: false),
                    Ham = table.Column<bool>(type: "bit", nullable: false),
                    Pepperoni = table.Column<bool>(type: "bit", nullable: false),
                    Mushrooms = table.Column<bool>(type: "bit", nullable: false),
                    Olives = table.Column<bool>(type: "bit", nullable: false),
                    CheeseType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bacon = table.Column<bool>(type: "bit", nullable: false),
                    Onions = table.Column<bool>(type: "bit", nullable: false),
                    GreenPeppers = table.Column<bool>(type: "bit", nullable: false),
                    Pineapple = table.Column<bool>(type: "bit", nullable: false),
                    Jalapenos = table.Column<bool>(type: "bit", nullable: false),
                    Anchovies = table.Column<bool>(type: "bit", nullable: false),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pizzas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pizzas");
        }
    }
}

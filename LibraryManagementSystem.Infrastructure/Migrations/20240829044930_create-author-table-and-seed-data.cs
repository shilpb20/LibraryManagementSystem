using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class createauthortableandseeddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Biography = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Biography", "Name" },
                values: new object[,]
                {
                    { 1, "Jane Austen was an English novelist known for her six major novels, including 'Pride and Prejudice' and 'Sense and Sensibility'. Her writing is known for its wit and social commentary.", "Jane Austen" },
                    { 2, "George Orwell was an English novelist and essayist, journalist, and critic. He is best known for his works 'Animal Farm' and '1984', which critique totalitarianism and social injustice.", "George Orwell" },
                    { 3, "Mark Twain was an American writer, humorist, and lecturer. He is famous for his novels 'The Adventures of Tom Sawyer' and 'Adventures of Huckleberry Finn', which provide a satirical view of society.", "Mark Twain" },
                    { 4, "Simon Sinek is an author and motivational speaker known for his book 'Start with Why'. He explores leadership and the importance of purpose in achieving success.", "Simon Sinek" },
                    { 5, "Malcolm Gladwell is a journalist and author known for his books 'Outliers' and 'The Tipping Point'. He explores the social and psychological factors behind success and change.", "Malcolm Gladwell" },
                    { 6, "Daniel Pink is an author and speaker known for his work on business and behavioral science. His books include 'Drive' and 'To Sell is Human', which discuss motivation and human behavior.", "Daniel Pink" },
                    { 7, "Brené Brown is a research professor and author known for her work on vulnerability, courage, and empathy. Her books include 'Daring Greatly' and 'Braving the Wilderness'.", "Brené Brown" },
                    { 8, "Adam Grant is an organizational psychologist and author known for his books 'Give and Take' and 'Think Again'. He explores the dynamics of generosity and rethinking in the workplace.", "Adam Grant" },
                    { 9, "Jim Collins is a business consultant and author known for his books 'Good to Great' and 'Built to Last'. He studies the principles that help companies achieve long-term success.", "Jim Collins" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}

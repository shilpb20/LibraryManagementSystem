using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class createdatabaseandauthorstablewithseededdata : Migration
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
                    Biography = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Biography", "CreatedAt", "LastModifiedAt", "Name" },
                values: new object[,]
                {
                    { 1, "Jane Austen was an English novelist known for her six major novels, including 'Pride and Prejudice' and 'Sense and Sensibility'. Her writing is known for its wit and social commentary.", new DateTime(2024, 8, 29, 17, 2, 44, 481, DateTimeKind.Local).AddTicks(9274), new DateTime(2024, 8, 29, 17, 2, 44, 481, DateTimeKind.Local).AddTicks(9338), "Jane Austen" },
                    { 2, "George Orwell was an English novelist and essayist, journalist, and critic. He is best known for his works 'Animal Farm' and '1984', which critique totalitarianism and social injustice.", new DateTime(2024, 8, 29, 17, 2, 44, 481, DateTimeKind.Local).AddTicks(9343), new DateTime(2024, 8, 29, 17, 2, 44, 481, DateTimeKind.Local).AddTicks(9345), "George Orwell" },
                    { 3, "Mark Twain was an American writer, humorist, and lecturer. He is famous for his novels 'The Adventures of Tom Sawyer' and 'Adventures of Huckleberry Finn', which provide a satirical view of society.", new DateTime(2024, 8, 29, 17, 2, 44, 481, DateTimeKind.Local).AddTicks(9349), new DateTime(2024, 8, 29, 17, 2, 44, 481, DateTimeKind.Local).AddTicks(9352), "Mark Twain" },
                    { 4, "Simon Sinek is an author and motivational speaker known for his book 'Start with Why'. He explores leadership and the importance of purpose in achieving success.", new DateTime(2024, 8, 29, 17, 2, 44, 481, DateTimeKind.Local).AddTicks(9355), new DateTime(2024, 8, 29, 17, 2, 44, 481, DateTimeKind.Local).AddTicks(9358), "Simon Sinek" },
                    { 5, "Malcolm Gladwell is a journalist and author known for his books 'Outliers' and 'The Tipping Point'. He explores the social and psychological factors behind success and change.", new DateTime(2024, 8, 29, 17, 2, 44, 481, DateTimeKind.Local).AddTicks(9362), new DateTime(2024, 8, 29, 17, 2, 44, 481, DateTimeKind.Local).AddTicks(9364), "Malcolm Gladwell" },
                    { 6, "Daniel Pink is an author and speaker known for his work on business and behavioral science. His books include 'Drive' and 'To Sell is Human', which discuss motivation and human behavior.", new DateTime(2024, 8, 29, 17, 2, 44, 481, DateTimeKind.Local).AddTicks(9368), new DateTime(2024, 8, 29, 17, 2, 44, 481, DateTimeKind.Local).AddTicks(9370), "Daniel Pink" },
                    { 7, "Brené Brown is a research professor and author known for her work on vulnerability, courage, and empathy. Her books include 'Daring Greatly' and 'Braving the Wilderness'.", new DateTime(2024, 8, 29, 17, 2, 44, 481, DateTimeKind.Local).AddTicks(9374), new DateTime(2024, 8, 29, 17, 2, 44, 481, DateTimeKind.Local).AddTicks(9377), "Brené Brown" },
                    { 8, "Adam Grant is an organizational psychologist and author known for his books 'Give and Take' and 'Think Again'. He explores the dynamics of generosity and rethinking in the workplace.", new DateTime(2024, 8, 29, 17, 2, 44, 481, DateTimeKind.Local).AddTicks(9380), new DateTime(2024, 8, 29, 17, 2, 44, 481, DateTimeKind.Local).AddTicks(9383), "Adam Grant" },
                    { 9, "Jim Collins is a business consultant and author known for his books 'Good to Great' and 'Built to Last'. He studies the principles that help companies achieve long-term success.", new DateTime(2024, 8, 29, 17, 2, 44, 481, DateTimeKind.Local).AddTicks(9387), new DateTime(2024, 8, 29, 17, 2, 44, 481, DateTimeKind.Local).AddTicks(9389), "Jim Collins" }
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

using LibraryManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedAuthorsTable(modelBuilder);
        }

        private void SeedAuthorsTable(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().HasData(
              new Author { Id = 1, Name = "Jane Austen", Biography = "Jane Austen was an English novelist known for her six major novels, including 'Pride and Prejudice' and 'Sense and Sensibility'. Her writing is known for its wit and social commentary." },
              new Author { Id = 2, Name = "George Orwell", Biography = "George Orwell was an English novelist and essayist, journalist, and critic. He is best known for his works 'Animal Farm' and '1984', which critique totalitarianism and social injustice." },
              new Author { Id = 3, Name = "Mark Twain", Biography = "Mark Twain was an American writer, humorist, and lecturer. He is famous for his novels 'The Adventures of Tom Sawyer' and 'Adventures of Huckleberry Finn', which provide a satirical view of society." },
              new Author { Id = 4, Name = "Simon Sinek", Biography = "Simon Sinek is an author and motivational speaker known for his book 'Start with Why'. He explores leadership and the importance of purpose in achieving success." },
              new Author { Id = 5, Name = "Malcolm Gladwell", Biography = "Malcolm Gladwell is a journalist and author known for his books 'Outliers' and 'The Tipping Point'. He explores the social and psychological factors behind success and change." },
              new Author { Id = 6, Name = "Daniel Pink", Biography = "Daniel Pink is an author and speaker known for his work on business and behavioral science. His books include 'Drive' and 'To Sell is Human', which discuss motivation and human behavior." },
              new Author { Id = 7, Name = "Brené Brown", Biography = "Brené Brown is a research professor and author known for her work on vulnerability, courage, and empathy. Her books include 'Daring Greatly' and 'Braving the Wilderness'." },
              new Author { Id = 8, Name = "Adam Grant", Biography = "Adam Grant is an organizational psychologist and author known for his books 'Give and Take' and 'Think Again'. He explores the dynamics of generosity and rethinking in the workplace." },
              new Author { Id = 9, Name = "Jim Collins", Biography = "Jim Collins is a business consultant and author known for his books 'Good to Great' and 'Built to Last'. He studies the principles that help companies achieve long-term success." }
          );
        }
    }
}

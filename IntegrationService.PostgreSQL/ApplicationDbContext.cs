using Microsoft.EntityFrameworkCore;
using IntegrationService.Models;
using IntegrationService.Models.Entities;

namespace IntegrationService.PostgreSQL
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext()
		{
		}

		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<Book> Books { get; set; }
		public DbSet<BookAuthor> BookAuthors { get; set; }
		public DbSet<Author> Authors { get; set; }
	}
}
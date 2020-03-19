using IntegrationService.Models;
using Microsoft.EntityFrameworkCore;
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

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(ConnectionStrings.Current.Postgres);
			base.OnConfiguring(optionsBuilder);
		}

		public DbSet<Book> Books { get; set; }
		public DbSet<BookAuthor> BookAuthors { get; set; }
		public DbSet<Author> Authors { get; set; }
	}
}
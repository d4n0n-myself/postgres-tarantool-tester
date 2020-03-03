#nullable enable
using System;
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
			var connString = GetConnString();

			optionsBuilder.UseNpgsql(connString);
			base.OnConfiguring(optionsBuilder);
		}

		private static string GetConnString()
		{
			string? connString;
			try
			{
				connString = Environment.GetEnvironmentVariable("PG_CONNSTRING");

				if (string.IsNullOrEmpty(connString))
					throw new ArgumentException();
			}
			catch (Exception e)
			{
				Console.Error.WriteLine("Environment variable PG_CONNSTRING is not defined.");
				throw;
			}

			return connString;
		}

		public DbSet<Book> Books { get; set; }
		public DbSet<BookAuthor> BookAuthors { get; set; }
		public DbSet<Author> Authors { get; set; }
	}
}
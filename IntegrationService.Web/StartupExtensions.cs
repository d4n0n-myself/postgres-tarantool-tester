using IntegrationService.Models;
using Microsoft.Extensions.DependencyInjection;
using IntegrationService.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IntegrationService.Web
{
	public static class StartupExtensions
	{
		public static void ManagePostgresDb(this IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>();

			using var applicationDbContext = new ApplicationDbContext();
			applicationDbContext.Database.Migrate();
		}

		public static void SetConnectionStrings(this IServiceCollection services)
		{
			Startup.Configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.Build();

			ConnectionStrings.Current = new ConnectionStrings(
				Startup.Configuration.GetValue<string>("Postgres"),
				Startup.Configuration.GetValue<string>("Tarantool"));
		}
	}
}
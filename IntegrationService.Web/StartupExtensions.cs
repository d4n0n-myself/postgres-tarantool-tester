using Microsoft.Extensions.DependencyInjection;
using IntegrationService.PostgreSQL;
using Microsoft.EntityFrameworkCore;

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
	}
}
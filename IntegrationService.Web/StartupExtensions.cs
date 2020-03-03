using Microsoft.Extensions.DependencyInjection;
using IntegrationService.PostgreSQL;

namespace IntegrationService.Web
{
	public static class StartupExtensions
	{
		public static void RegisterPgContext(this IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>();
		}
	}
}
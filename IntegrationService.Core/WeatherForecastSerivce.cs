using System.Collections.Generic;
using IntegrationService.PostgreSQL;
using IntegrationService.Models;

namespace IntegrationService.Core
{
	public class WeatherForecastService
	{
		private readonly ApplicationDbContext _context;

		public WeatherForecastService(ApplicationDbContext context)
		{
			_context = context;
		}

		public IAsyncEnumerable<WeatherForecast> GetAll()
		{
			return _context.Forecasts.AsAsyncEnumerable();
		}
	}
}
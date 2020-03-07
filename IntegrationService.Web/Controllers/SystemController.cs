using System;
using System.Threading.Tasks;
using IntegrationService.PostgreSQL;
using IntegrationService.Tarantool;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IntegrationService.Web.Controllers
{
	[Route("[action]")]
	public class SystemController : Controller
	{
		private readonly ILogger<SystemController> _logger;

		public SystemController(ILogger<SystemController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public IActionResult Ping()
		{
			return Ok("Online");
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		[AcceptVerbs("GET", "POST", "PUT", "DELETE", "HEAD", "OPTIONS", "PATCH")]
		public IActionResult Error()
		{
			var handlerResult = HttpContext
				.Features
				.Get<IExceptionHandlerPathFeature>();

			if (handlerResult == null)
				return Problem("Smth went wrong. No stack trace availible.");
			
			_logger.LogError(handlerResult.Error,
				$"Unhandled error on {handlerResult.Path}:{Environment.NewLine} {handlerResult.Error}");
			return Conflict(new
			{
				handlerResult.Error.Message,
				RequestPath = handlerResult.Path
			});
		}

		[HttpPost]
		public async Task<IActionResult> ChangeDatabase()
		{
			if (Startup.CurrentDatabase == typeof(PostgresRepository))
			{
//				await TarantoolRepository.EnsureCreated();
				Startup.CurrentDatabase = typeof(TarantoolRepository);
			}
			else
			{
				Startup.CurrentDatabase = typeof(PostgresRepository);
			}

			return Ok(Startup.CurrentDatabase.Name);
		}
	}
}
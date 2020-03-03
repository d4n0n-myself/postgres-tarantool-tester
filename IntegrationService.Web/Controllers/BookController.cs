using IntegrationService.Models;
using IntegrationService.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationService.Web.Controllers
{
	[Route("[controller]/[action]")]
	public class BookController : BaseController<Book>
	{
		public BookController(IRepository repository) : base(repository)
		{
		}
	}
}
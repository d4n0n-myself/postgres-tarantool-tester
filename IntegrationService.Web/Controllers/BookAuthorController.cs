using IntegrationService.Models;
using IntegrationService.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationService.Web.Controllers
{
	[Route("[controller]/[action]")]
	public class BookAuthorController : BaseController<BookAuthor>
	{
		public BookAuthorController(IRepository repository) : base(repository)
		{
		}
	}
}
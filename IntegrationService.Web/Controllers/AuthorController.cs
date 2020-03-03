using IntegrationService.Models;
using IntegrationService.Models.Entities;
using Microsoft.AspNetCore.Components;

namespace IntegrationService.Web.Controllers
{
	[Route("[controller]/[action]")]
	public class AuthorController : BaseController<Author>
	{
		public AuthorController(IRepository repository) : base(repository)
		{
		}
	}
}
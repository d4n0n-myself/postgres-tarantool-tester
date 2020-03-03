using IntegrationService.Models;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationService.Web.Controllers
{
	public class BaseController<T> : Controller where T : class
	{
		private readonly IRepository _repository;

		public BaseController(IRepository repository)
		{
			_repository = repository;
		}

		public IActionResult Delete(long id)
		{
			_repository.Delete<T>(id);
			return Ok();
		}

		public IActionResult Get(long id)
		{
			_repository.Get<T>(id);
			return Ok();
		}

		public IActionResult Update(T entity)
		{
			_repository.Update(entity);
			return Ok();
		}

		public IActionResult Save(T entity)
		{
			_repository.Save(entity);
			return Ok();
		}
	}
}
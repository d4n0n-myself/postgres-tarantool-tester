using System.Threading.Tasks;
using IntegrationService.Models;
using IntegrationService.Models.Entities.Base;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationService.Web.Controllers
{
	[Route("[controller]/[action]")]
	public class BaseController<T> : Controller where T : BaseEntity
	{
		private readonly IRepository _repository;

		public BaseController(IRepository repository)
		{
			_repository = repository;
		}

		[HttpPost]
		public async Task<IActionResult> Delete(long id)
		{
			await _repository.DeleteAsync<T>(id);
			return Ok();
		}

		[HttpGet]
		public async Task<IActionResult> Get(long id)
		{
			var data = await _repository.GetAsync<T>(id);
			return Ok(data);
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var data = await _repository.GetAllAsync<T>();
			return Ok(data);
		}

		[HttpPost]
		public async Task<IActionResult> Update([FromBody] T entity)
		{
			await _repository.UpdateAsync(entity);
			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> Save([FromBody] T entity)
		{
			await _repository.SaveAsync(entity);
			return Ok();
		}
	}
}
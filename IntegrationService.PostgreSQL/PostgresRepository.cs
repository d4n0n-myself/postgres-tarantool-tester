using System.Threading.Tasks;
using IntegrationService.Models;
using IntegrationService.Models.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace IntegrationService.PostgreSQL
{
	public class PostgresRepository : IRepository
	{
		private readonly ApplicationDbContext _context;

		public PostgresRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<bool> DeleteAsync<T>(long id) where T : BaseEntity
		{
			await Task.CompletedTask;
			var find = _context.Find<T>(id);
			if (find != null)
			{
				_context.Remove(find);
				_context.SaveChanges();
			}

			return true;
		}

		public async Task<T> GetAsync<T>(long id) where T : BaseEntity
		{
			await Task.CompletedTask;
			var find = _context.Set<T>().Find(id);
			return find;
		}

		public async Task<T[]> GetAllAsync<T>() where T : BaseEntity
		{
			return await _context.Set<T>().ToArrayAsync();
		}

		public async Task<bool> SaveAsync<T>(T entity) where T : BaseEntity
		{
			await Task.CompletedTask;
			var entityEntry = _context.Add(entity);
			_context.SaveChanges();
			return true;
		}

		public async Task<bool> UpdateAsync<T>(T entity) where T : BaseEntity
		{
			await Task.CompletedTask;
			var entityEntry = _context.Update(entity);
			_context.SaveChanges();
			return true;
		}
	}
}
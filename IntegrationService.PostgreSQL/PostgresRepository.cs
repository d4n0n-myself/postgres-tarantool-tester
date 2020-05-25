using System;
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

		public async Task<bool> DeleteAsync<T>(long? id) where T : BaseEntity
		{
			var find = await _context.FindAsync<T>(id);
			if (find == null) 
				throw new ArgumentException("No data by given id.");
			
			_context.Remove(find);
			await _context.SaveChangesAsync();

			return true;
		}

		public async Task<T> GetAsync<T>(long? id) where T : BaseEntity
		{
			var find = await _context.Set<T>().FindAsync(id);
			return find;
		}

		public async Task<T[]> GetAllAsync<T>() where T : BaseEntity
		{
			return await _context.Set<T>().ToArrayAsync();
		}

		public async Task<bool> SaveAsync<T>(T entity) where T : BaseEntity
		{
			var entityEntry = await _context.AddAsync(entity);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> UpdateAsync<T>(T entity) where T : BaseEntity
		{
			var entityEntry = _context.Update(entity);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
using IntegrationService.Models;

namespace IntegrationService.PostgreSQL
{
	public class PostgresRepository : IRepository
	{
		private readonly ApplicationDbContext _context;

		public PostgresRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public bool Delete<T>(long id) where T : class
		{
			var find = _context.Find<T>(id);
			if (find != null)
			{
				_context.Remove(find);
				_context.SaveChanges();
			}
			
			return true;
		}

		public T Get<T>(long id) where T : class
		{
			var find = _context.Set<T>().Find(id);
			return find;
		}

		public bool Save<T>(T entity)
		{
			var entityEntry = _context.Add(entity);
			_context.SaveChanges();
			return true;
		}

		public bool Update<T>(T entity)
		{
			var entityEntry = _context.Update(entity);
			_context.SaveChanges();
			return true;
		}
	}
}
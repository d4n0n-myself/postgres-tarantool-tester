using System;
using IntegrationService.Models;

namespace IntegrationService.Tarantool
{
	public class TarantoolRepository : IRepository
	{
		public bool Delete<T>(long id) where T : class
		{
			throw new NotImplementedException();
		}

		public T Get<T>(long id) where T : class
		{
			throw new NotImplementedException();
		}

		public bool Save<T>(T entity)
		{
			throw new NotImplementedException();
		}

		public bool Update<T>(T entity)
		{
			throw new NotImplementedException();
		}
	}
}
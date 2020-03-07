using System;
using System.Threading.Tasks;
using IntegrationService.Models;
using ProGaudi.Tarantool.Client;

namespace IntegrationService.Tarantool
{
	public class TarantoolRepository : IRepository
	{
		private Box _box;
		private ISchema _schema;
		
		public TarantoolRepository()
		{
			Connect()
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult();
		}

//		public static async Task<bool> EnsureCreated()
//		{
//			var box = await Box.Connect("127.0.0.1:3301");
//			var schema = box.GetSchema();
//
//			var modelTypes = AppDomain.CurrentDomain
//				.GetAssemblies()
//				.Where(x => !x.IsDynamic)
//				.SelectMany(x => x.GetExportedTypes()
//					.Where(y => y.IsClass)
//					.Where(typeof(BaseEntity).IsAssignableFrom)
//					.ToArray())
//				.Where(x => x != typeof(BaseEntity))
//				.ToList();
//			var spaceNames = schema.Spaces.Select(x => x.Name).ToArray();
//			
//			foreach (var type in modelTypes)
//			{
//				if (spaceNames.Contains(type.Name.ToLower()))
//					continue;
//
//				await box.Call($"box.schema.space.create({type.Name.ToLower()})");
//
////				var result = await box.ExecuteSql($"CREATE TABLE {type.Name.ToLower()} ()");
//			}
//
//			return true;
//		}
		
		public async Task Connect()
		{
			_box = await Box.Connect("127.0.0.1:3301");
			_schema = _box.GetSchema();
		}
		
		public bool Delete<T>(long id) where T : class
		{
			throw new NotImplementedException();
		}

		
		public T Get<T>(long id) where T : class
		{
			return GetAsync<T>(id).GetAwaiter().GetResult();
		}

		public async Task<T> GetAsync<T>(long id) where T : class
		{
			var typeName = typeof(T).Name.ToLower();
			return await _schema[typeName].Get<long, T>(id);
		}

		public bool Save<T>(T entity)
		{
			return SaveAsync(entity).GetAwaiter().GetResult();
		}

		public async Task<bool> SaveAsync<T>(T entity)
		{
			var typeName = typeof(T).Name.ToLower();
			try
			{
				var insert = await _schema[typeName].Insert(new object());
				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		public bool Update<T>(T entity)
		{
			throw new NotImplementedException();
		}
	}
}
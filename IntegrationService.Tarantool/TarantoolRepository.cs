using System;
using System.Linq;
using System.Threading.Tasks;
using IntegrationService.Models;
using IntegrationService.Models.Entities;
using IntegrationService.Models.Entities.Base;
using ProGaudi.MsgPack.Light;
using ProGaudi.Tarantool.Client;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.UpdateOperations;

namespace IntegrationService.Tarantool
{
	public class TarantoolRepository : IRepository
	{
		private Box _box;
		private ISchema _schema;

		private string TypeName(Type type) => type.Name.ToLower();

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
			var msgPackContext = new MsgPackContext();

			msgPackContext.GenerateAndRegisterArrayConverter<Book>();
			msgPackContext.GenerateAndRegisterArrayConverter<Author>();
			msgPackContext.GenerateAndRegisterArrayConverter<BookAuthor>();

			var clientOptions = new ClientOptions(ConnectionStrings.Current.Tarantool, context: msgPackContext);
			_box = new Box(clientOptions);
			await _box.Connect();
			_schema = _box.GetSchema();
		}
		
		public async Task<bool> DeleteAsync<T>(long id) where T : BaseEntity
		{
			await _schema[TypeName(typeof(T))]["primary_id"].Delete<TarantoolTuple<long>, T>(TarantoolTuple.Create(id));

			return true;
		}

		public async Task<T> GetAsync<T>(long id) where T : BaseEntity
		{
			var primaryIndex = _schema[TypeName(typeof(T))]["primary_id"];
			var response = await primaryIndex.Select<TarantoolTuple<long>, T>(TarantoolTuple.Create(id),
				new SelectOptions {Iterator = Iterator.Eq});
			
			return response.Data.First();
		}

		public async Task<T[]> GetAllAsync<T>() where T : BaseEntity
		{
			var primaryIndex = _schema[TypeName(typeof(T))]["primary_id"];
			var response = await primaryIndex.Select<TarantoolTuple<long>, T>(TarantoolTuple.Create(-1L),
				new SelectOptions {Iterator = Iterator.All});

			return response.Data.ToArray();
		}
		
		public async Task<bool> SaveAsync<T>(T entity) where T : BaseEntity
		{
			await _schema[TypeName(typeof(T))]["primary_id"].Insert(entity);
			return true;
		}


		public async Task<bool> UpdateAsync<T>(T entity) where T : BaseEntity
		{
			await DeleteAsync<T>(entity.Id);

			await SaveAsync(entity);

			return true;
		}

		public async Task<bool> OldUpdateAsync<T>(T entity) where T : BaseEntity
		{
			var props = entity.GetType().GetProperties();
			var operations = new UpdateOperation[props.Length];

			for (var i = 0; i < props.Length; i++)
			{
				var propValue = props[i].GetValue(entity);
				var genericTypeOperation = typeof(UpdateOperation)
					.GetMethods()
					.Single(x => x.Name == "CreateAssign")
					.MakeGenericMethod(propValue.GetType())
					.Invoke(null, new[] { i, propValue});

				operations[i] = (UpdateOperation) genericTypeOperation;
			}

			await _schema[TypeName(typeof(T))]["primary_id"]
				.Update<T, TarantoolTuple<long>>(TarantoolTuple.Create(entity.Id), operations);

			return true;
		}
	}
}
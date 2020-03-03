namespace IntegrationService.Models
{
	public interface IRepository
	{
		bool Delete<T>(long id) where T : class;
		T Get<T>(long id) where T : class;
		bool Save<T>(T entity);
		bool Update<T>(T entity);
	}
}
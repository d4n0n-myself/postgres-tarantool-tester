using System.Threading.Tasks;
using IntegrationService.Models.Entities.Base;

namespace IntegrationService.Models
{
	public interface IRepository
	{
		Task<bool> DeleteAsync<T>(long id) where T : BaseEntity;
		Task<T> GetAsync<T>(long id) where T : BaseEntity;
		Task<T[]> GetAllAsync<T>() where T : BaseEntity;
		Task<bool> SaveAsync<T>(T entity) where T : BaseEntity;
		Task<bool> UpdateAsync<T>(T entity) where T : BaseEntity;
	}
}
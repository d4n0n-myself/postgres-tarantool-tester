using IntegrationService.Models.Entities.Base;

namespace IntegrationService.Models.Entities
{
	public class Author : BaseEntity
	{
		public string Name { get; set; }
		public string FamilyName { get; set; }
		public string MiddleName { get; set; }
	}
}
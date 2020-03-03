using IntegrationService.Models.Entities.Base;

namespace IntegrationService.Models.Entities
{
	public class BookAuthor : BaseEntity
	{
		public Book Book { get; set; }
		public Author Author { get; set; }
	}
}
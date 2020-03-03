using System;
using IntegrationService.Models.Entities.Base;

namespace IntegrationService.Models.Entities
{
	public class Book : BaseEntity
	{
		public string Name { get; set; }
		public DateTime ReleaseDate { get; set; }
		public int PagesCount { get; set; }
		public string Publisher { get; set; }
	}
}
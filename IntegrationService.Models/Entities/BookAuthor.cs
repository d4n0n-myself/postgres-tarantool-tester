using IntegrationService.Models.Entities.Base;
using ProGaudi.MsgPack.Light;

namespace IntegrationService.Models.Entities
{
	[MsgPackArray]
	public class BookAuthor : BaseEntity
	{
		[MsgPackArrayElement(1)] public Book Book { get; set; }
		[MsgPackArrayElement(2)] public Author Author { get; set; }
	}
}
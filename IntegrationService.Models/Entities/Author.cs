using IntegrationService.Models.Entities.Base;
using ProGaudi.MsgPack.Light;

namespace IntegrationService.Models.Entities
{
	[MsgPackArray]
	public class Author : BaseEntity
	{
		[MsgPackArrayElement(1)] public string Name { get; set; }
		[MsgPackArrayElement(2)] public string FamilyName { get; set; }
		[MsgPackArrayElement(3)] public string MiddleName { get; set; }
	}
}
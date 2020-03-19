using System;
using IntegrationService.Models.Entities.Base;
using ProGaudi.MsgPack.Light;

namespace IntegrationService.Models.Entities
{
	[MsgPackArray]
	public class Book : BaseEntity
	{
		[MsgPackArrayElement(1)] public string Name { get; set; }
		[MsgPackArrayElement(2)] public DateTime ReleaseDate { get; set; }
		[MsgPackArrayElement(3)] public int PagesCount { get; set; }
		[MsgPackArrayElement(4)] public string Publisher { get; set; }
	}
}
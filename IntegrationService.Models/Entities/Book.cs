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
		[MsgPackArrayElement(3)] public uint PagesCount { get; set; }
		[MsgPackArrayElement(4)] public string Publisher { get; set; }
		[MsgPackArrayElement(5)] public string Description { get; set; }
		[MsgPackArrayElement(6)] public string PublisherCity { get; set; }
		[MsgPackArrayElement(7)] public byte HeadersCount { get; set; }
		[MsgPackArrayElement(8)] public decimal Price { get; set; }
		[MsgPackArrayElement(9)] public string Category { get; set; }
	}
}
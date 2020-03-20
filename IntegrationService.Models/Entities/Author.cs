using System;
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
		[MsgPackArrayElement(4)] public byte Age { get; set; }
		[MsgPackArrayElement(5)] public string BirthCountry { get; set; }
		[MsgPackArrayElement(6)] public string BirthCity { get; set; }
		[MsgPackArrayElement(7)] public byte BooksCount { get; set; }
		[MsgPackArrayElement(8)] public string Nationality { get; set; }
		[MsgPackArrayElement(9)] public DateTime BirthDateTime { get; set; }
	}
}
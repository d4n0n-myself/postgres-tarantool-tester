using ProGaudi.MsgPack.Light;

namespace IntegrationService.Models.Entities.Base
{
	public abstract class BaseEntity
	{
		[MsgPackArrayElement(0)] 
		public long? Id { get; set; }
	}
}
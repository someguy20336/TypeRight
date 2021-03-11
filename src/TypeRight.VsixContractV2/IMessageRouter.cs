namespace TypeRight.VsixContractV2
{
	public interface IMessageRouter
	{
		IResponse Send(IRequest message);

		bool SupportsMessageType(string type);
	}
}

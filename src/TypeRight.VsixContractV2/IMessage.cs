namespace TypeRight.VsixContractV2
{
	public interface IMessage
	{
		string MessageType { get; }

		object[] Payload { get; }
	}

	public interface IRequest : IMessage { }
	public interface IResponse : IMessage { }
}

using TypeRight.VsixContractV2;

namespace TypeRight.VsixContract.Messages
{
	public class AddNewConfigFileRequest : MessageBase, IRequest
	{
		public const string MessageTypeValue = "AddNewConfigFileRequest";
		public override string MessageType => MessageTypeValue;

		public string ConfigPath => GetValueAs<string>(0);

		public AddNewConfigFileRequest(string configPath) : base(new object[] { configPath })
		{

		}

		private AddNewConfigFileRequest(IMessage message) : base(message)
		{

		}

		public static AddNewConfigFileRequest Read(IMessage message) => new AddNewConfigFileRequest(message);
	}

	public class AddNewConfigFileResponse : MessageBase, IResponse
	{
		public const string MessageTypeValue = "AddNewConfigFileResponse";
		public override string MessageType => MessageTypeValue;

		public AddNewConfigFileResponse() : base(new object[0])
		{

		}

		private AddNewConfigFileResponse(IMessage message) : base(message)
		{

		}

		public static AddNewConfigFileResponse Read(IMessage message) => new AddNewConfigFileResponse(message);
	}
}

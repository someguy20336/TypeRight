using TypeRight.VsixContractV2;

namespace TypeRight.VsixContract.Messages
{
	public class GetConfigFilePathRequest : MessageBase, IRequest
	{
		public const string MessageTypeValue = "GetConfigFilePathRequest";
		public override string MessageType => MessageTypeValue;

		public string ProjectPath => GetValueAs<string>(0);

		public GetConfigFilePathRequest(string projPath) : base(new object[] { projPath })
		{

		}

		private GetConfigFilePathRequest(IMessage message) : base(message)
		{

		}

		public static GetConfigFilePathRequest Read(IMessage message) => new GetConfigFilePathRequest(message);
	}

	public class GetConfigFilePathResponse : MessageBase, IResponse
	{
		public const string MessageTypeValue = "GetConfigFilePathResponse";
		public override string MessageType => MessageTypeValue;

		public string FilePath => GetValueAs<string>(0);

		public GetConfigFilePathResponse(string path) : base(new object[] { path })
		{

		}

		private GetConfigFilePathResponse(IMessage message) : base(message)
		{

		}

		public static GetConfigFilePathResponse Read(IMessage message) => new GetConfigFilePathResponse(message);
	}
}

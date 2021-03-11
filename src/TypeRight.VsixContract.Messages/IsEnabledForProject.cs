using TypeRight.VsixContractV2;

namespace TypeRight.VsixContract.Messages
{
	public class IsEnabledForProjectRequest : MessageBase, IRequest
	{
		public const string MessageTypeValue = "IsEnabledForProjectRequest";
		public override string MessageType => MessageTypeValue;

		public string ProjectPath => GetValueAs<string>(0);

		public IsEnabledForProjectRequest(string projPath) : base(new object[] { projPath})
		{

		}

		private IsEnabledForProjectRequest(IMessage message) : base(message)
		{

		}

		public static IsEnabledForProjectRequest Read(IMessage message) => new IsEnabledForProjectRequest(message);
	}

	public class IsEnabledForProjectResponse : MessageBase, IResponse
	{
		public const string MessageTypeValue = "IsEnabledForProjectResponse";
		public override string MessageType => MessageTypeValue;
		public bool IsEnabled => GetValueAs<bool>(0);

		public IsEnabledForProjectResponse(bool isEnabled) : base(new object[] { isEnabled })
		{

		}

		private IsEnabledForProjectResponse(IMessage message) : base(message)
		{

		}

		public static IsEnabledForProjectResponse Read(IMessage message) => new IsEnabledForProjectResponse(message);
	}
}

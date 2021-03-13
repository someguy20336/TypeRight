using TypeRight.VsixContractV2;

namespace TypeRight.VsixContract.Messages
{
	public class UpgradeConfigRequest : MessageBase, IRequest
	{
		public const string MessageTypeValue = "UpgradeConfigRequest";
		public override string MessageType => MessageTypeValue;

		public string ConfigPath => GetValueAs<string>(0);

		public UpgradeConfigRequest(string configPath) : base(new object[] { configPath })
		{

		}

		private UpgradeConfigRequest(IMessage message) : base(message)
		{

		}

		public static UpgradeConfigRequest Read(IMessage message) => new UpgradeConfigRequest(message);
	}

	public class UpgradeConfigResponse : MessageBase, IResponse
	{
		public const string MessageTypeValue = "UpgradeConfigResponse";
		public override string MessageType => MessageTypeValue;
		public bool CanUpgrade => GetValueAs<bool>(0);

		public UpgradeConfigResponse() : base(new object[0])
		{

		}

		private UpgradeConfigResponse(IMessage message) : base(message)
		{

		}

		public static UpgradeConfigResponse Read(IMessage message) => new UpgradeConfigResponse(message);
	}
}

using TypeRight.VsixContractV2;

namespace TypeRight.VsixContract.Messages
{
	public class CanUpgradeConfigRequest : MessageBase, IRequest
	{
		public const string MessageTypeValue = "CanUpgradeConfigRequest";
		public override string MessageType => MessageTypeValue;

		public string ConfigPath => GetValueAs<string>(0);

		public CanUpgradeConfigRequest(string configPath) : base(new object[] { configPath })
		{

		}

		private CanUpgradeConfigRequest(IMessage message) : base(message)
		{

		}

		public static CanUpgradeConfigRequest Read(IMessage message) => new CanUpgradeConfigRequest(message);
	}

	public class CanUpgradeConfigResponse : MessageBase, IResponse
	{
		public const string MessageTypeValue = "CanUpgradeConfigResponse";
		public override string MessageType => MessageTypeValue;
		public bool CanUpgrade => GetValueAs<bool>(0);

		public CanUpgradeConfigResponse(bool canUpgrade) : base(new object[] { canUpgrade })
		{

		}

		private CanUpgradeConfigResponse(IMessage message) : base(message)
		{

		}

		public static CanUpgradeConfigResponse Read(IMessage message) => new CanUpgradeConfigResponse(message);
	}
}

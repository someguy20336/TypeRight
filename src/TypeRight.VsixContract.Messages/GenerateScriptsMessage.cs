using Microsoft.CodeAnalysis;
using TypeRight.VsixContractV2;

namespace TypeRight.VsixContract.Messages
{
	public class GenerateScriptsMessage : MessageBase, IRequest
	{
		public const string MessageTypeValue = "GenerateScriptsRequest";
		public override string MessageType => MessageTypeValue;

		public Workspace Workspace => GetValueAs<Workspace>(0);
		public string ProjectPath => GetValueAs<string>(1);
		public bool Force => GetValueAs<bool>(2);

		private GenerateScriptsMessage(Workspace workspace, string projPath, bool force)
			: base(new object[] { workspace, projPath, force })
		{

		}

		private GenerateScriptsMessage(IMessage message) : base(message)
		{

		}

		public static GenerateScriptsMessage Create(Workspace workspace, string projPath, bool force)
			=> new GenerateScriptsMessage(workspace, projPath, force);

		public static GenerateScriptsMessage Read(IMessage message) => new GenerateScriptsMessage(message);

	}

	public class GenerateScriptsResponse : MessageBase, IResponse
	{
		public const string MessageTypeValue = "GenerateScriptsResponse";
		public override string MessageType => MessageTypeValue;

		public bool Success => GetValueAs<bool>(0);
		public string ErrorMessage => GetValueAs<string>(1);

		public GenerateScriptsResponse(bool success, string errorMessage)
			: base(new object[] { success, errorMessage })
		{
		}

		private GenerateScriptsResponse(IMessage message) : base(message)
		{

		}


		public static GenerateScriptsResponse Read(IMessage message) => new GenerateScriptsResponse(message);
	}
}

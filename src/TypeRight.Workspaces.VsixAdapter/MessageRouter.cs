using System.Collections.Generic;
using System.ComponentModel.Composition;
using TypeRight.VsixContract.Messages;
using TypeRight.VsixContractV2;

namespace TypeRight.Workspaces.VsixAdapter
{

	[Export(typeof(IMessageRouter))]
	public class MessageRouter : IMessageRouter
	{
		private delegate IResponse MessageHandler(IRequest request);

		private Dictionary<string, MessageHandler> _handlers = new Dictionary<string, MessageHandler>()
		{
			{  GenerateScriptsRequest.MessageTypeValue, req => ScriptGenerationAdapter.GenerateScripts(req) },
			{  AddNewConfigFileRequest.MessageTypeValue, req => Configuration.CreateNew(req) },
			{  IsEnabledForProjectRequest.MessageTypeValue, req => Configuration.IsEnabled(req) },
			{  GetConfigFilePathRequest.MessageTypeValue, req => Configuration.GetConfigFilepath(req) }
		};


		public IResponse Send(IRequest message)
		{
			if (!_handlers.TryGetValue(message.MessageType, out var handler))
			{
				return null;
			}

			return handler(message);
		}

		public bool SupportsMessageType(string type)
		{
			return _handlers.ContainsKey(type);
		}
	}
}

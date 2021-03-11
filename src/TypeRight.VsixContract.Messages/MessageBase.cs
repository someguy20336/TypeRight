using System;
using System.Collections.Generic;
using System.Linq;
using TypeRight.VsixContractV2;

namespace TypeRight.VsixContract.Messages
{
	public abstract class MessageBase : IMessage
	{
		private object[] _payload;
		public abstract string MessageType { get; }

		object[] IMessage.Payload => _payload;

		protected MessageBase(IEnumerable<object> payload)
		{
			_payload = payload.ToArray();
		}

		protected MessageBase(IMessage fromMessage)
		{
			if (MessageType != fromMessage.MessageType)
			{
				throw new InvalidOperationException("Cannot copy payload from other message type");
			}

			_payload = fromMessage.Payload.ToArray();
		}

		protected T GetValueAs<T>(int index) => (T)_payload[index];
	}
}

using System;
using Sisk.Utils.Net.Delegates;
using Sisk.Utils.Net.Messages;

namespace Sisk.Utils.Net.Wrapper {
    internal class MessageHandlerWrapper {
        private Action<ulong, object> Action { get; set; }
        private Func<Wrapper, object> DeserializeAction { get; set; }
        public int HashCode { get; private set; }

        public static MessageHandlerWrapper Create<TMessageType>(MessageHandler<TMessageType> handler) where TMessageType : IMessage {
            return new MessageHandlerWrapper { Action = (sender, message) => handler(sender, (TMessageType) message), HashCode = handler.Method.GetHashCode(), DeserializeAction = wrapper => Wrapper.GetContent<TMessageType>(wrapper) };
        }

        public static MessageHandlerWrapper Create<TMessageType>(EntityMessageHandler<TMessageType> handler) where TMessageType : IEntityMessage {
            return new MessageHandlerWrapper { Action = (sender, message) => handler(sender, (TMessageType) message), HashCode = handler.Method.GetHashCode(), DeserializeAction = wrapper => Wrapper.GetContent<TMessageType>(wrapper) };
        }

        public object Deserialize(MessageWrapper wrapper) {
            return DeserializeAction(wrapper);
        }

        public object Deserialize(EntityMessageWrapper wrapper) {
            return DeserializeAction(wrapper);
        }

        public void Invoke(ulong sender, object message) {
            Action(sender, message);
        }
    }
}
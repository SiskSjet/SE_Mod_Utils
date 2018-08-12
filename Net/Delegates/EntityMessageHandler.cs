using Sisk.Utils.Net.Messages;

namespace Sisk.Utils.Net.Delegates {
    public delegate void EntityMessageHandler<in TMessageType>(ulong sender, TMessageType message) where TMessageType : IEntityMessage;
}
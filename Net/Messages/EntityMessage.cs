using ProtoBuf;
using Sandbox.ModAPI;
using Sisk.Utils.Net.Wrapper;

// ReSharper disable ExplicitCallerInfoArgument

namespace Sisk.Utils.Net.Messages {
    [ProtoContract]
    internal class EntityMessage : IMessage {
        [ProtoMember(1)]
        public EntityMessageWrapper Wrapper { get; set; }

        public byte[] Serialize() {
            return MyAPIGateway.Utilities.SerializeToBinary(this);
        }
    }
}
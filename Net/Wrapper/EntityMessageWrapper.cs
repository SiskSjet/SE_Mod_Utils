using System;
using ProtoBuf;

// ReSharper disable ExplicitCallerInfoArgument

namespace Sisk.Utils.Net.Wrapper {
    [ProtoContract]
    internal sealed class EntityMessageWrapper : Wrapper {
        public EntityMessageWrapper() { }

        public EntityMessageWrapper(string type) {
            if (string.IsNullOrWhiteSpace(type)) {
                throw new ArgumentNullException(nameof(type), $"'{nameof(type)}' can not be null or whitespace.");
            }

            Type = type;
        }

        [ProtoMember(1)]
        public long EntityId { get; set; }

        [ProtoMember(2)]
        public string Type { get; set; }
    }
}
namespace Sisk.Utils.Net.Messages {
    public interface IEntityMessage {
        long EntityId { get; set; }
        byte[] Serialze();
    }
}
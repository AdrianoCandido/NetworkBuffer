
namespace NetworkBuffer.Messaging.Serialization
{
    public interface IMessageSerializer
    {
        byte[] Pack(IMessage replyMessage);
    }
}
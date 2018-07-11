
namespace NetworkBuffer.Communication.Messaging.Serialization
{
    /// <summary>
    /// Represents the serializer.
    /// </summary>
    public interface IMessageSerializer
    {
        /// <summary>
        /// Packs the specified  message.
        /// </summary>
        /// <param name="message">The message to pack.</param>
        /// <returns></returns>
        byte[] Pack(IMessage message);


        /// <summary>
        /// Unpacks the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="size">The size.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        IMessage Unpack(byte[] buffer, int size, int index = 0);
    }
}
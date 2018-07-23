namespace NetworkBuffer.Communication.Messaging.Serialization
{
    /// <summary>
    /// Represents the serializer.
    /// </summary>
    public interface IMessageSerializer
    {
        /// <summary>
        /// Packs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        byte[] Pack(IMessage message);

        /// <summary>
        /// Tries the pack.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        bool TryPack(IMessage message, out byte[] result);

        /// <summary>
        /// Unpacks the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="size">The size.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        IMessage Unpack(byte[] buffer, int size, int index);

        /// <summary>
        /// Tries the unpack.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="size">The size.</param>
        /// <param name="index">The index.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        bool TryUnpack(byte[] buffer, int size, int index, out IMessage result);
    }
}
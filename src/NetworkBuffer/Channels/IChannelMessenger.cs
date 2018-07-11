using NetworkBuffer.Communication.Messaging;
using System.Threading.Tasks;

namespace NetworkBuffer.Channels
{
    /// <summary>
    /// The transporter for the message received in the channel.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message.</typeparam>
    public interface IChannelMessenger
    {
        /// <summary>
        /// Send the message to the client.
        /// </summary>
        /// <param name="replyMessage">Message to reply to the client.</param>
        /// <returns>If message as sent to client.</returns>
        Task ReplyMessageAsync(IMessage replyMessage);

        /// <summary>
        /// The message received in the channel.
        /// </summary>
        IMessage Message { get; }
    }
}
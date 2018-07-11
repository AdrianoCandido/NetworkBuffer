using NetworkBuffer.Communication.Messaging;
using System;
using System.Threading.Tasks;

namespace NetworkBuffer.Channels
{
    /// <summary>
    /// Transport message between client and service.
    /// </summary>
    public class ChannelMessenger : IChannelMessenger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelMessenger"/> class.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        /// <param name="message">The message.</param>
        /// <param name="notifier">The notifier.</param>
        /// <param name="networkClient">The network client.</param>
        public ChannelMessenger(IMessage message, IChannelNotifier notifier)
        {
            this.Message = message ?? throw new ArgumentNullException(nameof(message));
            this.Notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
        }

        /// <summary>
        /// The message received in the channel.
        /// </summary>
        public IMessage Message { get; }

        /// <summary>
        /// Gets the notifier.
        /// </summary>
        private IChannelNotifier Notifier { get; }

        /// <summary>
        /// Send the message to the client.
        /// </summary>
        /// <param name="replyMessage">Message to reply to the client.</param>
        /// <returns>
        /// If message as sent to client.
        /// </returns>
        public async Task ReplyMessageAsync(IMessage replyMessage)
        {
            this.Notifier.NotifySend(replyMessage);
        }
    }
}
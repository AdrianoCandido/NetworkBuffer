using System;
using System.Threading.Tasks;
using NetworkBuffer.Communication.Tcp;
using NetworkBuffer.Messaging;
using NetworkBuffer.Messaging.Serialization;

namespace NetworkBuffer.Channels
{
    /// <summary>
    /// Transport message between client and service.
    /// </summary>
    public class ChannelMessenger : IChannelMessenger
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelMessenger"/> class.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        /// <param name="message">The message.</param>
        /// <param name="notifier">The notifier.</param>
        /// <param name="networkClient">The network client.</param>
        public ChannelMessenger(IMessageSerializer serializer, IMessage message, IChannelNotifier notifier, INetworkClient networkClient)
        {
            this.Serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            this.Message = message ?? throw new ArgumentNullException(nameof(message));
            this.Client = networkClient ?? throw new ArgumentNullException(nameof(networkClient));
            this.Notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets the notifier.
        /// </summary>
        private IChannelNotifier Notifier { get; }

        /// <summary>
        /// Gets or sets the data serializer.
        /// </summary>
        private IMessageSerializer Serializer { get; }

        /// <summary>
        /// Client that sent or receive the message.
        /// </summary>
        private INetworkClient Client { get; }

        #endregion

        #region Members of IChannelMessenger

        /// <summary>
        /// The message received in the channel.
        /// </summary>
        public IMessage Message { get; }

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

            if (replyMessage == null)
                throw new ArgumentNullException(nameof(replyMessage));

            // Pack the message to reply.
            byte[] result = this.Serializer.Pack(replyMessage);

            // Send the message to client.
            await this.Client.SendSync(result);
        }

        #endregion
    }
}
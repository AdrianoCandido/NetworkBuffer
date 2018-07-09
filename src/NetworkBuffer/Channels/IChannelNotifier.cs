using System;
using System.Collections.Generic;
using NetworkBuffer.Communication.Tcp;
using NetworkBuffer.Messaging;
using NetworkBuffer.Messaging.Serialization;

namespace NetworkBuffer.Channels
{
    /// <summary>
    /// Notify a message received and sent in the channel.
    /// </summary>
    public interface IChannelNotifier
    {
        /// <summary>
        /// Event raised on channel entry receive a message.
        /// </summary>
        event EventHandler<IMessage> ReceiveMessage;

        /// <summary>
        /// Event raised on channel entry send a message.
        /// </summary>
        event EventHandler<IMessage> SendMessage;

        /// <summary>
        /// Notifies the send.
        /// </summary>
        /// <param name="message">The data list.</param>
        void NotifySend(IMessage message);

        /// <summary>
        /// Notifies the received.
        /// </summary>
        /// <param name="message">The data list.</param>
        void NotifyReceived(IMessage message);

        /// <summary>
        /// Gets the message serializer.
        /// </summary>
        IMessageSerializer MessageSerializer { get; }

        /// <summary>
        /// Client that sent or receive the message.
        /// </summary>
        INetworkClient NetworkClient { get; }
    }
}
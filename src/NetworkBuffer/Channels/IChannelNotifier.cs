using NetworkBuffer.Communication.Messaging;
using System;

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
        /// Occurs when [connection lost].
        /// </summary>
        event EventHandler ConnectionLost;

        /// <summary>
        /// Notifies the connection lost.
        /// </summary>
        /// <param name="message">The message.</param>
        void NotifyConnectionLost();

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
    }
}
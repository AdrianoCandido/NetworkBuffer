using NetworkBuffer.Communication.Messaging;
using System;

namespace NetworkBuffer.Channels
{
    /// <summary>
    /// Notifier  channel event.
    /// </summary>
    /// <seealso cref="Dlp.Buy4.HostSimulator.Simulator.Channels.IChannelNotifier" />
    public class ChannelNotifier : IChannelNotifier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelNotifier"/> class.
        /// </summary>
        public ChannelNotifier()
        {
        }

        /// <summary>
        /// Event raised on channel entry receive a message.
        /// </summary>
        public event EventHandler<IMessage> ReceiveMessage;

        /// <summary>
        /// Event raised on channel entry send a message.
        /// </summary>
        public event EventHandler<IMessage> SendMessage;

        /// <summary>
        /// Occurs when [connection lost].
        /// </summary>
        public event EventHandler ConnectionLost;

        /// <summary>
        /// Notifies the connection lost.
        /// </summary>
        public void NotifyConnectionLost()
        {
            ConnectionLost.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Notifies the received.
        /// </summary>
        /// <param name="dataList">The data list.</param>
        public void NotifyReceived(IMessage message)
        {
            this.ReceiveMessage?.Invoke(this, message);
        }

        /// <summary>
        /// Notifies the send.
        /// </summary>
        /// <param name="message">The data list.</param>
        public void NotifySend(IMessage message)
        {
            this.SendMessage?.Invoke(this, message);
        }
    }
}
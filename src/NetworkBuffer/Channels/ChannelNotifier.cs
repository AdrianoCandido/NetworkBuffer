using System;
using NetworkBuffer.Communication.Tcp;
using NetworkBuffer.Messaging;
using NetworkBuffer.Messaging.Serialization;

namespace NetworkBuffer.Channels
{
    /// <summary>
    /// Notifier  channel event.
    /// </summary>
    /// <seealso cref="Dlp.Buy4.HostSimulator.Simulator.Channels.IChannelNotifier" />
    public class ChannelNotifier : IChannelNotifier
    {
        #region Public Constructors

        public ChannelNotifier(IMessageSerializer serializer, INetworkClient client)
        {
            this.MessageSerializer = serializer;
            this.NetworkClient = client;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Event raised on channel entry receive a message.
        /// </summary>
        public event EventHandler<IMessage> ReceiveMessage;

        /// <summary>
        /// Event raised on channel entry send a message.
        /// </summary>
        public event EventHandler<IMessage> SendMessage;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the message serializer.
        /// </summary>
        public IMessageSerializer MessageSerializer { get; }

        /// <summary>
        /// Client that sent or receive the message.
        /// </summary>
        public INetworkClient NetworkClient { get; }

        #endregion

        #region Public Methods

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
        /// <param name="dataList">The data list.</param>
        public void NotifySend(IMessage dataList)
        {
            this.SendMessage?.Invoke(this, dataList);
        }

        #endregion
    }
}
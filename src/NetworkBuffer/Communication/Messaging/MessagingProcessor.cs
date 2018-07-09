using NetworkBuffer.Channels;
using NetworkBuffer.Messaging.Serialization;
using System;

namespace NetworkBuffer.Messaging
{
    /// <summary>
    /// Process the message
    /// </summary>
    public abstract class MessagingProcessor
    {
        /// <summary>
        /// Gets or sets the channel notifier.
        /// </summary>
        private IChannelNotifier ChannelNotifier { get; set; }

        /// <summary>
        /// Parser to split  messages received.
        /// </summary>
        private IMessageParser Parser { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagingProcessor"/> class.
        /// </summary>
        /// <param name="channelNotifier">The channel notifier.</param>
        /// <exception cref="ArgumentNullException">channelNotifier</exception>
        public MessagingProcessor(IChannelNotifier channelNotifier, IMessageParser dataParser)
        {
            this.ChannelNotifier = channelNotifier ?? throw new ArgumentNullException(nameof(channelNotifier));
            this.Parser = dataParser ?? throw new ArgumentNullException(nameof(dataParser));

            this.Parser.MessageFound += this.OnDataListFound;
        }

        /// <summary>
        /// Puts the data to data parser.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="size">The size.</param>
        public void PutData(byte[] buffer, int size)
        {
            this.Parser.Put(buffer, 0, size);
        }

        /// <summary>
        /// Called when [data list found].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DataListFoundEventArgs"/> instance containing the event data.</param>
        private void OnDataListFound(object sender, Message e)
        {
            this.ChannelNotifier.NotifyReceived(e);
        }
    }
}
using NetworkBuffer.Communication.Tcp;
using NetworkBuffer.Messaging;
using System;

namespace NetworkBuffer.Channels
{
    /// <summary>
    /// Logical controller for the single client.
    /// </summary>
    public sealed class Channel<TChannelController> : IChannel<TChannelController> where TChannelController : class, IChannelController
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Channel{TController}"/> class.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="networkClient">The e.</param>
        public Channel(TChannelController controller, INetworkClient networkClient, MessagingProcessor messagingProcessor)
        {
            this.InitializeChannel(controller, networkClient, messagingProcessor);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Logical controller to the channel.
        /// </summary>
        public TChannelController ChannelController { get; private set; }

        /// <summary>
        /// The connected socket in the channel.
        /// </summary>
        public INetworkClient Client { get; private set; }

        /// <summary>
        /// Channel event notifier.
        /// </summary>
        public IChannelNotifier Notifier { get; private set; }

        #endregion

        #region Private Properties

        /// <summary>
        /// Processor for message protocol.
        /// </summary>
        private MessagingProcessor MessagingProcessor { get; set; }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the channel.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="networkClient">The network client.</param>
        private async void InitializeChannel(TChannelController controller, INetworkClient networkClient, MessagingProcessor messagingProcessor)
        {

            this.ChannelController = controller ?? throw new ArgumentNullException(nameof(controller));
            ChannelConfiguration configuration = controller.GetConfiguration();

            this.Client = networkClient ?? throw new ArgumentNullException(nameof(networkClient));
            this.Notifier = new ChannelNotifier(configuration.Serializer, this.Client);
            this.Notifier.ReceiveMessage += (object sender, IMessage message) => this.ChannelController.ProcessMessage(new ChannelMessenger(configuration.Serializer, message, this.Notifier, this.Client));
            this.MessagingProcessor = messagingProcessor ?? throw new ArgumentNullException(nameof(messagingProcessor));
            this.Client.DataReceived += (object sender, DataReceivedEventArgs e) => this.MessagingProcessor.PutData(e.Buffer, e.Size);

            await this.Client.Initialize();
        }

        private void Notifier_ReceiveMessage(object sender, IMessage e)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
using NetworkBuffer.Communication.Messaging;
using NetworkBuffer.Communication.Messaging.Serialization;
using NetworkBuffer.Communication.Tcp;
using System;

namespace NetworkBuffer.Channels
{
    /// <summary>
    /// Logical controller for the single client.
    /// </summary>
    public class Channel<TChannelController> : IChannel where TChannelController : class, IChannelController
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Channel{TController}"/> class.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="networkClient">The e.</param>
        public Channel(INetworkClient networkClient, IMessageParser messageParser, IMessageSerializer messageSerializer, ChannelControllerFactory<TChannelController> channelControllerFactory = null)
        {
            this.ChannelControllerFactory = channelControllerFactory ?? new ChannelControllerFactory<TChannelController>();
            this.InitializeNetworkClient(networkClient);
            this.InitializeSerializer(messageSerializer);
            this.InitializeNotifier();
            this.InitializeParser(messageParser);
            this.InitializeController();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Logical controller to the channel.
        /// </summary>
        public virtual IChannelController ChannelController { get; private set; }

        /// <summary>
        /// Gets the message parser.
        /// </summary>
        public virtual IMessageParser MessageParser { get; private set; }

        /// <summary>
        /// Gets or sets the message serializer.
        /// </summary>
        public virtual IMessageSerializer MessageSerializer { get; private set; }

        /// <summary>
        /// Gets or sets the network client.
        /// </summary>
        public virtual INetworkClient NetworkClient { get; private set; }

        /// <summary>
        /// Channel event notifier.
        /// </summary>
        public virtual IChannelNotifier Notifier { get; private set; }

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets the channel controller factory.
        /// </summary>
        private ChannelControllerFactory<TChannelController> ChannelControllerFactory { get; }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the controller.
        /// </summary>
        private void InitializeController()
        {
            this.ChannelController = this.ChannelControllerFactory.CrateController();
            this.ChannelController.NetworkClient = this.NetworkClient;
            this.NetworkClient.Initialize();
            this.ChannelController.Initialize(this.NetworkClient);
        }

        /// <summary>
        /// Initializes the network client.
        /// </summary>
        /// <param name="networkClient">The network client.</param>
        private void InitializeNetworkClient(INetworkClient networkClient)
        {
            this.NetworkClient = networkClient ?? throw new ArgumentNullException(nameof(networkClient));
            this.NetworkClient.ConnectionLost += (object source, EventArgs e) => this.Notifier.NotifyConnectionLost();
            this.NetworkClient.DataReceived += (object sender, DataReceivedEventArgs e) => this.MessageParser.PutData(e.Buffer, e.Size);
        }

        /// <summary>
        /// Initializes the notifier.
        /// </summary>
        private void InitializeNotifier()
        {
            this.Notifier = new ChannelNotifier();
            this.Notifier.ReceiveMessage += (object sender, IMessage message) => this.ChannelController.ProcessMessage(new ChannelMessenger(message, this.Notifier));
            this.Notifier.SendMessage += (object source, IMessage message) => this.NetworkClient.SendSync(this.MessageSerializer.Pack(message));
        }

        /// <summary>
        /// Initializes the parser.
        /// </summary>
        /// <param name="messageParser">The message parser.</param>
        private void InitializeParser(IMessageParser messageParser)
        {
            this.MessageParser = messageParser ?? throw new ArgumentNullException(nameof(messageParser));
            this.MessageParser.MessageFound += (object source, MessageFoundEventArgs e) => this.Notifier.NotifyReceived(e.Message);
        }

        /// <summary>
        /// Initializes the serializer.
        /// </summary>
        /// <param name="messageSerializer">The message serializer.</param>
        private void InitializeSerializer(IMessageSerializer messageSerializer)
        {
            this.MessageSerializer = messageSerializer ?? throw new ArgumentNullException(nameof(messageSerializer));
        }

        #endregion
    }
}
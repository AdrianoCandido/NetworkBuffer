using NetworkBuffer.Channels;
using NetworkBuffer.Communication;
using NetworkBuffer.Communication.Messaging.Serialization;
using NetworkBuffer.Communication.Tcp;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace NetworkBuffer
{
    /// <summary>
    /// Provider the communication channel for the specified controller.
    /// </summary>
    public class HostService<TController> where TController : class, IChannelController
    {
        #region Public Constructors

        /// <summary>
        /// Create a new instance of the channel.
        /// </summary>
        /// <param name="channelController">Controller used to process the client messages.</param>
        /// <exception cref="ArgumentException"><paramref name="channelController"/></exception>
        public HostService(ProtocolBinding binding)
        {
            this.Binding = binding ?? throw new ArgumentNullException(nameof(binding));
            this.Binding.Listener.NetworkClientAccepted += AcceptNetworkClient;
            this.ChannelList = new ObservableCollection<Channel<TController>>();
            this.Channels = new ReadOnlyObservableCollection<Channel<TController>>(this.ChannelList);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the binding.
        /// </summary>
        public ProtocolBinding Binding { get; }

        /// <summary>
        /// List of channels.
        /// </summary>
        public ReadOnlyObservableCollection<Channel<TController>> Channels { get; }

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets or sets the channels.
        /// </summary>
        private ObservableCollection<Channel<TController>> ChannelList { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start this channel.
        /// </summary>
        public async Task<HostService<TController>> StartAsync()
        {
            await this.Binding.InitializeAsync();
            return this;
        }

        /// <summary>
        /// Stop this channel.
        /// </summary>
        public void Stop()
        {
            this.Binding.Listener.Stop();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Event rises on server accept client.
        /// </summary>
        /// <param name="sender">Object that send the event.</param>
        /// <param name="networkClient">Client connected.</param>
        private void AcceptNetworkClient(object sender, ClientAcceptedEventArgs e)
        {
            IMessageParser messageParser = this.Binding.CreateParser();
            IMessageSerializer messageSerializer = this.Binding.MessageSerializer;
            Channel<TController> channel = new Channel<TController>(e.NetworkClient, messageParser, messageSerializer);
            this.ChannelList.Add(channel);
        }

        #endregion
    }
}
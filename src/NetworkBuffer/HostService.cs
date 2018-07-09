using NetworkBuffer.Channels;
using NetworkBuffer.Communication;
using NetworkBuffer.Communication.Tcp;
using NetworkBuffer.Messaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetworkBuffer
{
    /// <summary>
    /// Provider the communication channel for the specified controller.
    /// </summary>
    public class HostService<TController> : IDisposable where TController : class, IChannelController
    {
        #region Public Constructors

        /// <summary>
        /// Create a new instance of the channel.
        /// </summary>
        /// <param name="channelController">Controller used to process the client messages.</param>
        /// <exception cref="ArgumentException"><paramref name="channelController"/></exception>
        public HostService(TController controller, MessagingProcessor messagingProcessor, Binding binding)
        {
            this.Binding = binding ?? throw new ArgumentNullException(nameof(binding));
            this.MessagingProcessor = messagingProcessor ?? throw new ArgumentNullException(nameof(messagingProcessor));
            this.Controller = controller ?? throw new ArgumentException(nameof(controller));
            this.Binding.Listener.ClientAccepted += AcceptNetworkClient;
            this.Channels = new List<Channel<TController>>();
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets or sets the binding.
        /// </summary>
        public Binding Binding { get; set; }

        /// <summary>
        /// List of channels.
        /// </summary>
        private List<Channel<TController>> Channels { get; }

        /// <summary>
        /// Logical controller for the channel
        /// </summary>
        private TController Controller { get; }

        /// <summary>
        /// Gets or sets the messaging processor.
        /// </summary>
        private MessagingProcessor MessagingProcessor { get; set; }



        #endregion

        #region Public Methods

        /// <summary>
        /// Dispose the object.
        /// </summary>
        public void Dispose()
        {
        }

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
        private void AcceptNetworkClient(object sender, INetworkClient networkClient)
        {
            Channel<TController> channel = new Channel<TController>(this.Controller, networkClient, this.MessagingProcessor);

            this.Channels.Add(channel);

            // Register channel remove event.
            networkClient.ConnectionLost += (object source, EventArgs args) => this.Channels.Remove(channel);
        }

        #endregion
    }
}
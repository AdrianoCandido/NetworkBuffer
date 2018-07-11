using NetworkBuffer.Communication.Tcp;
using System;
using System.Threading.Tasks;

namespace NetworkBuffer.Channels
{
    /// <summary>
    /// Interface to create logical implementation for the channel.
    /// </summary>
    public interface IChannelController
    {
        /// <summary>
        /// Gets the network client.
        /// </summary>
        INetworkClient NetworkClient { get; set; }

        /// <summary>
        /// Initialize the controller.
        /// </summary>
        /// <param name="messenger"></param>
        void Initialize(INetworkClient client);

        /// <summary>
        /// Process one message received in the channel.
        /// </summary>
        /// <param name="messenger">Message sent by client.</param>
        Task ProcessMessage(IChannelMessenger messenger);
    }
}
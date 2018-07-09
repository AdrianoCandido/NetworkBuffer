using NetworkBuffer.Channels;
using NetworkBuffer.Communication.Tcp;
using System;
using System.Threading.Tasks;

namespace NetworkBuffer.Channels
{
    /// <summary>
    /// Interface to create logical implementation for the channel.
    /// </summary>
    public interface IChannelController : IDisposable
    {
        /// <summary>
        /// Get configuration for the channel
        /// </summary>
        /// <returns>The channel configuration</returns>
        ChannelConfiguration GetConfiguration();

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
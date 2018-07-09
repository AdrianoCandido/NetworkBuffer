using NetworkBuffer.Communication.Tcp;

namespace NetworkBuffer.Channels
{
    /// <summary>
    /// Represents the channel.
    /// </summary>
    /// <typeparam name="TController">The type of the controller.</typeparam>
    public interface IChannel<TController> where TController : class, IChannelController
    {
        /// <summary>
        /// Logical controller for the channel.
        /// </summary>
        TController ChannelController { get; }

        /// <summary>
        /// Client to communication.
        /// </summary>
        INetworkClient Client { get; }
    }
}
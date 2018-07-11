using NetworkBuffer.Communication.Messaging.Serialization;
using System.Net;

namespace NetworkBuffer.Channels
{
    /// <summary>
    /// Define the configuration for the channel.
    /// </summary>
    public class ChannelConfiguration
    {
        #region Public Properties

        /// <summary>
        /// Listener endPoint.
        /// </summary>
        public IPEndPoint EndPoint { get; set; }

        /// <summary>
        /// Name of the channel.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get the serializer to the channel.
        /// </summary>
        public IMessageSerializer Serializer { get; set; }

        #endregion
    }
}
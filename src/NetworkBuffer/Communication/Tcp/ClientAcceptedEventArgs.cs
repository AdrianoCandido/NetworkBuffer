using System;

namespace NetworkBuffer.Communication.Tcp
{
    /// <summary>
    /// Represents The client accepted event arguments.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ClientAcceptedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientAcceptedEventArgs"/> class.
        /// </summary>
        /// <param name="networkClient">The network client.</param>
        public ClientAcceptedEventArgs(INetworkClient networkClient)
        {
            this.NetworkClient = networkClient;
        }

        /// <summary>
        /// Gets the network client.
        /// </summary>
        public INetworkClient NetworkClient { get; }
    }
}
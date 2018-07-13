using System;
using System.Threading.Tasks;

namespace NetworkBuffer.Communication.Tcp
{
    /// <summary>
    /// Network client to stream communication.
    /// </summary>
    public interface INetworkClient
    {
        /// <summary>
        /// Disconnect current client.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Start read client data.
        /// </summary>
        Task Initialize();

        /// <summary>
        /// Send bytes to client;
        /// </summary>
        /// <param name="buffer">Write the buffer on client stream.</param>
        Task SendSync(byte[] buffer);

        /// <summary>
        /// Event occurred on client or server lost the connection.
        /// </summary>
        event EventHandler ConnectionLost;

        /// <summary>
        /// Called on Receive data from client.
        /// </summary>
        event EventHandler<DataReceivedEventArgs> DataReceived;
    }
}
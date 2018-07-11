using NetworkBuffer.Communication.Tcp;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NetworkBuffer.Communication
{
    /// <summary>
    /// Represent the listener.
    /// </summary>
    public interface IListener
    {
        /// <summary>
        /// Event called on receive clients
        /// </summary>
        event EventHandler<ClientAcceptedEventArgs> NetworkClientAccepted;

        /// <summary>
        /// The listener end point.
        /// </summary>
        IPEndPoint IPEndPoint { get; }

        /// <summary>
        /// Gets a value that indicates whether System.Net.Sockets.TcpListener is actively listening for client connections.
        /// </summary>
        /// <returns>true if System.Net.Sockets.TcpListener is actively listening; otherwise, false.</returns>
        bool IsActive { get; }
        
        /// <summary>
        /// Starts the service.
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// Stops the service.
        /// </summary>
        void Stop();
    }
}
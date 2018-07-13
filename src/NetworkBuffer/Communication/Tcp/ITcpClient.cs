using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace NetworkBuffer.Communication.Tcp
{
    /// <summary>
    ///
    /// </summary>
    public interface ITcpClient : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="ITcpClient"/> is connected.
        /// </summary>
        bool Connected { get; }

        /// <summary>
        /// Gets the available.
        /// </summary>
        int Available { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ITcpClient"/> is active.
        /// </summary>
        bool Active { get; }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        void Close();

        /// <summary>
        /// Connects the asynchronous.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        Task ConnectAsync(IPAddress address, int port);

        /// <summary>
        /// Connects the asynchronous.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        Task ConnectAsync(string host, int port);

        /// <summary>
        /// Gets the stream.
        /// </summary>
        /// <returns></returns>
        Stream Stream();
    }
}
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NetworkBuffer.Communication.Tcp
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="NetworkBuffer.Communication.Tcp.ITcpClient" />
    public class TcpClientWrapper : ITcpClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TcpClientWrapper"/> class.
        /// </summary>
        /// <param name="tcpClient">The TCP client.</param>
        /// <exception cref="ArgumentNullException">tcpClient</exception>
        public TcpClientWrapper(TcpClient tcpClient)
        {
            this.TcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ITcpClient" /> is connected.
        /// </summary>
        public bool Connected => this.TcpClient.Connected;

        /// <summary>
        /// Gets the available.
        /// </summary>
        public int Available => this.TcpClient.Available;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ITcpClient" /> is active.
        /// </summary>
        public bool Active => Convert.ToBoolean(this.TcpClient.GetType().GetProperty("Active", System.Reflection.BindingFlags.NonPublic).GetValue(this.TcpClient));

        /// <summary>
        /// Gets the TCP client.
        /// </summary>
        /// <value>
        /// The TCP client.
        /// </value>
        public TcpClient TcpClient { get; }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        public void Close()
        {
            this.TcpClient.Close();
        }

        /// <summary>
        /// Connects the asynchronous.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        public Task ConnectAsync(IPAddress address, int port)
        {
            return this.TcpClient.ConnectAsync(address, port);
        }

        /// <summary>
        /// Connects the asynchronous.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        public Task ConnectAsync(string host, int port)
        {
            return this.TcpClient.ConnectAsync(host, port);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.TcpClient.Dispose();
        }

        /// <summary>
        /// Gets the stream.
        /// </summary>
        /// <returns></returns>
        public Stream Stream()
        {
            return this.TcpClient.GetStream();
        }
    }
}
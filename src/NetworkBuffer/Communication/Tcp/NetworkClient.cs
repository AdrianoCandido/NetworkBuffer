using NetworkBuffer.Communication.Tcp;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkBuffer.Communication.Tcp
{
    /// <summary>
    /// Network client to stream communication.
    /// </summary>
    public class NetworkClient : INetworkClient
    {
        #region Public Constructors

        /// <summary>
        /// Create communication on TCP clients.
        /// </summary>
        /// <param name="client">Client start transmission.</param>
        public NetworkClient(TcpClient client, IListener listenner)
        {
            this.Listener = listenner;
            this.TcpClient = client ?? throw new ArgumentNullException(nameof(client));
            this.Stream = this.TcpClient.GetStream();
            this.TokenSource = new CancellationTokenSource();
        }

        public IListener Listener { get; }

        #endregion

        #region Public Events

        /// <summary>
        /// Event occurred on client or server lost the connection.
        /// </summary>
        public event EventHandler ConnectionLost;

        /// <summary>
        /// Called on Receive data from client.
        /// </summary>
        public event EventHandler<DataReceivedEventArgs> DataReceived;

        #endregion

        #region Private Properties

        /// <summary>
        /// Handle for client communication
        /// </summary>
        private TcpClient TcpClient { get; }

        /// <summary>
        /// Token to manage current activity.
        /// </summary>
        private CancellationTokenSource TokenSource { get; }

        /// <summary>
        /// Connection stream.
        /// </summary>
        private NetworkStream Stream { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Disconnect current client.
        /// </summary>
        public void Disconnect()
        {
            try
            {
                this.TokenSource.Cancel();
                this.TcpClient?.Client?.Shutdown(SocketShutdown.Both);
                this.TcpClient?.Close();
                this.Stream?.Dispose();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Start read client data.
        /// </summary>
        public async Task Initialize()
        {
            await this.ReadDataAsync();
        }

        /// <summary>
        /// Send bytes to client;
        /// </summary>
        /// <param name="buffer">Write the buffer on client stream.</param>
        public async Task SendSync(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            await this.Stream.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Invoke Event Connection lost.
        /// </summary>
        private void DoConnectionLost()
        {
            this.Disconnect();
            this.ConnectionLost?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Read client data async.
        /// </summary>
        private async Task ReadDataAsync()
        {
            try
            {
                while (this.TcpClient.Connected)
                {
                    byte[] buffer = new byte[1024];

                    int size = await this.Stream.ReadAsync(buffer, 0, buffer.Length, TokenSource.Token);

                    if (size == 0)
                    {
                        this.DoConnectionLost();
                    }

                    this.DataReceived?.Invoke(this, new DataReceivedEventArgs(size, buffer));
                }

                this.DoConnectionLost();
            }
            catch (Exception e)
            {
                this.DoConnectionLost();
            }
        }

        #endregion
    }
}
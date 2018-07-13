using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkBuffer.Communication.Tcp
{
    /// <summary>
    /// Network client to stream communication.
    /// </summary>
    public class NetworkClient : INetworkClient, IDisposable
    {
        #region Public Constructors

        /// <summary>
        /// Create communication on TCP clients.
        /// </summary>
        /// <param name="client">Client start transmission.</param>
        public NetworkClient(ITcpClient client)
        {
            this.TcpClient = client ?? throw new ArgumentNullException(nameof(client));
            this.Stream = this.TcpClient.Stream();
            this.TokenSource = new CancellationTokenSource();
        }

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
        /// Connection stream.
        /// </summary>
        private Stream Stream { get; }

        /// <summary>
        /// Handle for client communication
        /// </summary>
        private ITcpClient TcpClient { get; }

        /// <summary>
        /// Token to manage current activity.
        /// </summary>
        private CancellationTokenSource TokenSource { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Disconnect current client.
        /// </summary>
        public void Disconnect()
        {
            try
            {
                this.TokenSource?.Cancel();
                this.TcpClient?.Close();
                this.Stream?.Dispose();
                this.ConnectionLost?.Invoke(this, EventArgs.Empty);
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
        /// Read client data async.
        /// </summary>
        private async Task ReadDataAsync()
        {
            using (ITcpClient client = this.TcpClient)
            {
                try
                {
                    while (client.Connected && this.TokenSource.IsCancellationRequested == false)
                    {
                        byte[] buffer = new byte[1024];

                        int size = await this.Stream.ReadAsync(buffer, 0, buffer.Length, TokenSource.Token);

                        if (size == 0)
                        {
                            this.Disconnect();
                            return;
                        }

                        this.DataReceived?.Invoke(this, new DataReceivedEventArgs(size, buffer));
                    }

                    this.Disconnect();
                }
                catch
                {
                    this.Disconnect();
                }
            }
        }

        /// <summary>
        /// The disposed
        /// </summary>
        private bool Disposed = false;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (Disposed == false)
            {
                this.Dispose(true);
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="dispose">
        ///   <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool dispose)
        {
            if (dispose)
            {
                this.Disposed = true;
                this.TokenSource?.Dispose();
                this.Stream?.Dispose();
                this.TcpClient?.Dispose();
            }
        }

        #endregion
    }
}
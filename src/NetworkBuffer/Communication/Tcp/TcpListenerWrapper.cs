using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NetworkBuffer.Communication.Tcp
{
    /// <summary>
    /// Wrapper for TcpListener
    /// </summary>
    public class TcpListenerWrapper : TcpListener, IListener
    {
        #region Public Constructors

        /// <summary>
        /// Constructor with local endPoint.
        /// </summary>
        /// <param name="localEP"></param>
        public TcpListenerWrapper(IPEndPoint localEP) : base(localEP)
        {
            this.IPEndPoint = localEP;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Event called on receive clients
        /// </summary>
        public event EventHandler<INetworkClient> ClientAccepted;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the IP end point.
        /// </summary>
        public IPEndPoint IPEndPoint { get; set; }

        /// <summary>
        /// Gets a value that indicates whether System.Net.Sockets.TcpListener is actively listening for client connections.
        /// </summary>
        /// <returns>true if System.Net.Sockets.TcpListener is actively listening; otherwise, false.</returns>
        public bool IsActive { get { return this.Active; } }

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the service.
        /// </summary>
        /// <returns></returns>
        public async Task InitializeAsync()
        {
            this.Start();
            await DoListemSocketAsync();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Accepts the TCP client asynchronous.
        /// </summary>
        /// <returns></returns>
        private async Task<INetworkClient> AcceptNetworkClientAsync()
        {
            return new NetworkClient(await this.AcceptTcpClientAsync(), this);
        }

        /// <summary>
        /// Start listener service.
        /// </summary>
        private async Task DoListemSocketAsync()
        {
            while (this.IsActive)
            {
                // Call client accepted event.
                this.ClientAccepted?.Invoke(this, await this.AcceptNetworkClientAsync());
            }
        }

        #endregion
    }
}
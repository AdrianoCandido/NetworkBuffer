using NetworkBuffer.Communication.Tcp;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NetworkBuffer.Communication.Tcp
{
    /// <summary>
    /// Simple TCP server.
    /// </summary>
    public class TcpServer : IDisposable
    {
        #region Public Constructors

        /// <summary>
        /// Constructor to Assign the service on all IPAdress.
        /// </summary>
        /// <param name="endpoint">EndPoint to listener.</param>
        /// <param name="name">Service name.</param>
        public TcpServer(IPEndPoint endpoint)
        {
            this.LocalEndPoint = new IPEndPoint(endpoint.Address.MapToIPv4(), endpoint.Port);
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
        /// End point to listener this service.
        /// </summary>
        public IPEndPoint LocalEndPoint { get; }

        #endregion

        #region Private Properties

        /// <summary>
        /// Listener to receive clients.
        /// </summary>
        private IListener Listener { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Dispose the channel objects.
        /// </summary>
        public void Dispose()
        {
            this.Listener.Stop();
            this.Dispose();
        }


        /// <summary>
        /// Stop this service
        /// </summary>
        public virtual void Stop()
        {
            this.Listener.Stop();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Initialize this service
        /// </summary>
        public async virtual Task InitializeSocketAsync()
        {
            this.Listener = new TcpListenerWrapper(LocalEndPoint);
            this.Listener.Start();
            await this.DoListemSocketAsync();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Start listener service.
        /// </summary>
        private async Task DoListemSocketAsync()
        {
            while (this.Listener.IsActive)
            {
                // Call client accepted event.
                this.ClientAccepted?.Invoke(this, await this.Listener.AcceptNetworkClientAsync());
            }
        }

        #endregion
    }
}
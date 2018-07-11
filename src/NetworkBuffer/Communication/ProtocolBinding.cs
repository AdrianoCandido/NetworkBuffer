using NetworkBuffer.Communication.Messaging.Serialization;
using System.Threading.Tasks;

namespace NetworkBuffer.Communication
{
    /// <summary>
    /// Represent the transport layer protocol binding.
    /// </summary>
    public abstract class ProtocolBinding
    {
        /// <summary>
        /// Gets or sets the listener.
        /// </summary>
        public virtual IListener Listener { get; }

        /// <summary>
        /// Gets the message serializer.
        /// </summary>
        public virtual IMessageSerializer MessageSerializer { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProtocolBinding"/> class.
        /// </summary>
        /// <param name="listener">The listener.</param>
        public ProtocolBinding()
        {
            this.Listener = this.CreateListener();
            this.MessageSerializer = this.CreateMessageSerializer();
        }

        /// <summary>
        /// Creates the message parser.
        /// </summary>
        /// <returns></returns>
        public abstract IMessageParser CreateParser();

        /// <summary>
        /// Creates the listener.
        /// </summary>
        /// <returns>The listener instance.</returns>
        protected abstract IListener CreateListener();

        /// <summary>
        /// Creates the message serializer.
        /// </summary>
        /// <returns></returns>
        protected abstract IMessageSerializer CreateMessageSerializer();

        /// <summary>
        /// Initializes the asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task InitializeAsync()
        {
            await this.Listener.InitializeAsync();
        }
    }
}
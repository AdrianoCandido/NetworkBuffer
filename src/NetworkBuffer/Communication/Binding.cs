using System.Threading.Tasks;

namespace NetworkBuffer.Communication
{
    /// <summary>
    /// Represent the transport layer protocol binding.
    /// </summary>
    public abstract class Binding
    {
        /// <summary>
        /// Gets or sets the listener.
        /// </summary>
        /// <value>
        /// The listener.
        /// </value>
        public IListener Listener { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Binding"/> class.
        /// </summary>
        /// <param name="listener">The listener.</param>
        public Binding(IListener listener)
        {
            this.Listener = listener ?? throw new System.ArgumentNullException(nameof(listener));
        }

        /// <summary>
        /// Initializes the asynchronous.
        /// </summary>
        /// <returns></returns>
        public virtual async Task InitializeAsync()
        {
            await Listener.InitializeAsync();
        }
    }
}
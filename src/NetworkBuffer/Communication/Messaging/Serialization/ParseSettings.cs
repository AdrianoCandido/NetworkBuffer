namespace NetworkBuffer.Communication.Messaging.Serialization
{
    /// <summary>
    /// Represents The parse settings.
    /// </summary>
    public class ParseSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseSettings"/> class.
        /// </summary>
        public ParseSettings(IMessageSerializer messageSerializer)
        {
            this.Serializer = messageSerializer ?? throw new System.ArgumentNullException(nameof(messageSerializer));
        }

        /// <summary>
        /// Gets or sets the serializer.
        /// </summary>
        public IMessageSerializer Serializer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is big-endian.
        /// </summary>
        public bool IsBigEndian { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [size inclusive].
        /// </summary>
        public bool SizeInclusive { get; set; }
    }
}
using System;

namespace NetworkBuffer.Communication.Messaging.Serialization
{
    /// <summary>
    /// Represents The message found event arguments.
    /// </summary>
    public class MessageFoundEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageFoundEventArgs"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MessageFoundEventArgs(IMessage message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public IMessage Message { get; }
    }
}
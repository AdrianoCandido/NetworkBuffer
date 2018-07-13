using System.Collections.Generic;

namespace NetworkBuffer.Communication.Messaging
{
    /// <summary>
    /// Represents the message.
    /// </summary>
    public class Message : Dictionary<string, string>, IMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        public Message()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        public Message(IDictionary<string, string> dictionary) : base(dictionary)
        {
        }
    }
}
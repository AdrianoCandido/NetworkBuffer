using NetworkBuffer.Messaging;
using System.Collections.Generic;

namespace NetworkBuffer.Messaging
{
    /// <summary>
    /// Represents the message.
    /// </summary>
    public class Message : Dictionary<string, string>, IMessage
    {
    }
}
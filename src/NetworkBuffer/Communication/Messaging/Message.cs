using NetworkBuffer.Communication.Messaging;
using System.Collections.Generic;

namespace NetworkBuffer.Communication.Messaging
{
    /// <summary>
    /// Represents the message.
    /// </summary>
    public class Message : Dictionary<string, string>, IMessage
    {
    }
}
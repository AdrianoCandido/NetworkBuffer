using System.Collections.Generic;

namespace NetworkBuffer.Communication.Messaging
{
    public interface IMessage : IDictionary<string, string>
    {
    }
}
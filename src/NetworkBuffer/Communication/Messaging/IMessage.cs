using System.Collections.Generic;

namespace NetworkBuffer.Messaging
{
    public interface IMessage : IDictionary<string, string>
    {
    }
}
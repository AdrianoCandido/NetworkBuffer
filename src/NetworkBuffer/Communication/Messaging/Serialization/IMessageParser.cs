using System;

namespace NetworkBuffer.Messaging.Serialization
{
    public interface IMessageParser
    {
        event EventHandler<Message> MessageFound;

        event EventHandler<byte[]> DataDiscarded;

        void Put(byte[] buffer, int value, int size);
    }
}
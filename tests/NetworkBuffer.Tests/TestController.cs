using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetworkBuffer.Channels;
using NetworkBuffer.Communication.Tcp;
using System.Threading.Tasks;

namespace NetworkBuffer.Tests
{
    public class TestController : IChannelController
    {
        public INetworkClient NetworkClient { get; set; }

        public void Dispose()
        {
        }

        public void Initialize(INetworkClient client)
        {
        }

        public Task ProcessMessage(IChannelMessenger messenger)
        {
            return Task.CompletedTask;
        }


       
    }
}
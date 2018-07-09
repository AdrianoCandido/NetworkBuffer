using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetworkBuffer.Channels;
using NetworkBuffer.Communication;
using NetworkBuffer.Communication.Tcp;
using NetworkBuffer.Messaging;
using NetworkBuffer.Messaging.Serialization;
using System.Threading.Tasks;

namespace NetworkBuffer.Tests
{
    [TestClass]
    public class ServiceFactoryTests
    {
        #region Mocks

        private class MockController : IChannelController
        {
            public void Dispose()
            {
            }

            public ChannelConfiguration GetConfiguration()
            {
                return new ChannelConfiguration();
            }

            public void Initialize(INetworkClient client)
            {
            }

            public Task ProcessMessage(IChannelMessenger messenger)
            {
                return Task.CompletedTask;
            }
        }

        private class MessageProcessorMock : MessagingProcessor
        {
            public MessageProcessorMock(IChannelNotifier channelNotifier, IMessageParser dataParser) : base(channelNotifier, dataParser)
            {
            }
        }

        private class BindingMock : Binding
        {
            public BindingMock(IListener listener) : base(listener)
            {
            }
        }

        #endregion

        private MessageProcessorMock processor;
        private MockController controller;
        private Binding binding;

        private Mock<IMessageSerializer> messageSerializerMock;
        private Mock<INetworkClient> networkClientMock;
        private Mock<IMessageParser> messageParserMock;
        private Mock<IListener> listenerMock;

        [TestInitialize]
        public void TestInitialize()
        {
            listenerMock = new Mock<IListener>();
            messageParserMock = new Mock<IMessageParser>();
            messageSerializerMock = new Mock<IMessageSerializer>();
            networkClientMock = new Mock<INetworkClient>();

            this.binding = new BindingMock(this.listenerMock.Object);
            this.controller = new MockController();
            this.processor = new MessageProcessorMock(channelNotifier: new ChannelNotifier(serializer: this.messageSerializerMock.Object,
                                                                                           client: this.networkClientMock.Object),
                                                      dataParser: this.messageParserMock.Object);
        }

        [TestMethod]
        public async Task Service_should_be_created()
        {
            HostService<MockController> hostService = await ServiceFactory.CreateService(this.controller, this.processor, this.binding);
            hostService.Should().NotBeNull();
        }
    }
}
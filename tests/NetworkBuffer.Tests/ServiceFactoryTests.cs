using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using NetworkBuffer.Communication;
using NetworkBuffer.Communication.Messaging.Serialization;
using NetworkBuffer.Communication.Tcp;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace NetworkBuffer.Tests
{
    [TestClass]
    public class ServiceFactoryTests
    {
        private Mock<ProtocolBinding> ProtocolBindingMock;
        private Mock<IMessageParser> MessageParserMock;
        private Mock<IMessageSerializer> MessageSerializerMock;
        private Mock<INetworkClient> NetworkClientMock;
        private Mock<IListener> ListenerMock;

        [TestInitialize]
        public void TestInitialize()
        {
            this.NetworkClientMock = new Mock<INetworkClient>();
            this.MessageParserMock = new Mock<IMessageParser>();
            this.MessageSerializerMock = new Mock<IMessageSerializer>();
            this.ListenerMock = new Mock<IListener>();
            this.ListenerMock.Setup(c => c.IsActive).Returns(true);

            this.ProtocolBindingMock = new Mock<ProtocolBinding>();
            this.ProtocolBindingMock.Setup(c => c.Listener).Returns(this.ListenerMock.Object);
            this.ProtocolBindingMock.Setup(c => c.MessageSerializer).Returns(this.MessageSerializerMock.Object);
            this.ProtocolBindingMock.Setup(c => c.CreateParser()).Returns(this.MessageParserMock.Object);
            this.ProtocolBindingMock.Protected().Setup<IListener>("CreateListener").Returns(this.ListenerMock.Object);
            this.ProtocolBindingMock.Protected().Setup<IMessageSerializer>("CreateMessageSerializer").Returns(this.MessageSerializerMock.Object);
        }

        [TestMethod]
        public async Task Service_should_be_created()
        {
            HostService<TestController> hostService = await ServiceFactory.CreateService<TestController>(this.ProtocolBindingMock.Object);
            hostService.Should().NotBeNull();
            hostService.Binding.Listener.IsActive.Should().BeTrue();
        }

        [TestMethod]
        public async Task Service_should_notify_collection_changed()
        {
            HostService<TestController> hostService = await ServiceFactory.CreateService<TestController>(this.ProtocolBindingMock.Object);
            INotifyCollectionChanged notifyCollectionChanged = hostService.Channels;
            bool changed = false;

            notifyCollectionChanged.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) =>
            {
                changed = true;
            };

            this.ListenerMock.Raise(c => c.NetworkClientAccepted += null, new ClientAcceptedEventArgs(this.NetworkClientMock.Object));
            changed.Should().BeTrue();
        }
    }
}
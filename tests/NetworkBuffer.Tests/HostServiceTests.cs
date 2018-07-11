using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using NetworkBuffer.Communication;
using NetworkBuffer.Communication.Messaging.Serialization;
using NetworkBuffer.Communication.Tcp;
using System;
using System.Threading.Tasks;

namespace NetworkBuffer.Tests
{
    [TestClass]
    public class HostServiceTests
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
        public async Task Host_service_should_be_starts()
        {
            HostService<TestController> hostService = await new HostService<TestController>(this.ProtocolBindingMock.Object).StartAsync();
            hostService.Should().NotBeNull();
            hostService.Channels.Should().NotBeNull();
            hostService.Channels.Should().HaveCount(0);
        }

        [TestMethod]
        public async Task Host_service_should_initialize_Listener()
        {
            this.ProtocolBindingMock.Setup(c => c.Listener.InitializeAsync()).Returns(Task.CompletedTask).Verifiable();
            await new HostService<TestController>(this.ProtocolBindingMock.Object).StartAsync();
            this.ProtocolBindingMock.Verify();
        }

        [TestMethod]
        public async Task Host_service_should_stop_listener()
        {
            this.ProtocolBindingMock.Setup(c => c.Listener.Stop()).Verifiable();
            HostService<TestController> hostService = await new HostService<TestController>(this.ProtocolBindingMock.Object).StartAsync();
            hostService.Stop();
            this.ProtocolBindingMock.Verify();
        }

        [TestMethod]
        public async Task Host_service_should_create_a_channel_when_client_connect()
        {
            HostService<TestController> hostService = await new HostService<TestController>(this.ProtocolBindingMock.Object).StartAsync();
            this.ListenerMock.Raise(c => c.NetworkClientAccepted += null, new ClientAcceptedEventArgs(this.NetworkClientMock.Object));
            hostService.Channels.Should().HaveCount(1);
        }

        [TestMethod]
        public void Host_service_should_be_throw_when_constructor_argument_is_null()
        {
            Action action = () => new HostService<TestController>(null);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
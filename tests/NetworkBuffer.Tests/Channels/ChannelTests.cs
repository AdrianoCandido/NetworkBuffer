using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using NetworkBuffer.Channels;
using NetworkBuffer.Communication;
using NetworkBuffer.Communication.Messaging;
using NetworkBuffer.Communication.Messaging.Serialization;
using NetworkBuffer.Communication.Tcp;
using System;
using System.Threading.Tasks;

namespace NetworkBuffer.Tests.Channels
{
    [TestClass]
    public class ChannelTests
    {
        private Mock<ProtocolBinding> ProtocolBindingMock;
        private Mock<IMessageParser> MessageParserMock;
        private Mock<IMessageSerializer> MessageSerializerMock;
        private Mock<INetworkClient> NetworkClientMock;
        private Mock<IListener> ListenerMock;
        private Mock<IChannelController> ChannelControllerMock;
        private Mock<ChannelControllerFactory<TestController>> ChannelControllerFactoryMock;
        private Channel<TestController> Channel;

        [TestInitialize]
        public void TestInitialize()
        {
            this.ChannelControllerMock = new Mock<IChannelController>();
            this.ChannelControllerFactoryMock = new Mock<ChannelControllerFactory<TestController>>();
            this.ChannelControllerFactoryMock.Setup(c => c.CrateController()).Returns(this.ChannelControllerMock.Object);
            this.NetworkClientMock = new Mock<INetworkClient>();
            this.MessageParserMock = new Mock<IMessageParser>();
            this.MessageSerializerMock = new Mock<IMessageSerializer>();
            byte[] result;
            this.MessageSerializerMock.Setup(c => c.TryPack(It.IsAny<IMessage>(), out result)).Returns(true);
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
        public void Channel_should_be_created()
        {
            this.Channel = new Channel<TestController>(this.NetworkClientMock.Object, this.ProtocolBindingMock.Object.CreateParser(), this.ProtocolBindingMock.Object.MessageSerializer);
            this.Channel.Should().NotBeNull();
            this.Channel.ChannelController.Should().NotBeNull();
            this.Channel.ChannelController.NetworkClient.Should().NotBeNull();
            this.Channel.MessageParser.Should().NotBeNull();
            this.Channel.MessageSerializer.Should().NotBeNull();
            this.Channel.NetworkClient.Should().NotBeNull();
            this.Channel.Notifier.Should().NotBeNull();
        }

        [TestMethod]
        public void Channel_should_initialize_network_client()
        {
            this.NetworkClientMock.Setup(c => c.Initialize()).Returns(Task.CompletedTask).Verifiable();
            this.Channel = new Channel<TestController>(this.NetworkClientMock.Object, this.ProtocolBindingMock.Object.CreateParser(), this.ProtocolBindingMock.Object.MessageSerializer);
            this.NetworkClientMock.VerifyAll();
        }

        [TestMethod]
        public void Channel_should_initialize_channel_controller()
        {
            this.ChannelControllerMock.Setup(c => c.Initialize(It.IsAny<INetworkClient>())).Verifiable();
            this.Channel = new Channel<TestController>(this.NetworkClientMock.Object, this.ProtocolBindingMock.Object.CreateParser(), this.ProtocolBindingMock.Object.MessageSerializer, this.ChannelControllerFactoryMock.Object);
            this.ChannelControllerMock.VerifyAll();
        }

        [TestMethod]
        public void Channel_should_notify_connection_lost()
        {
            bool changed = false;
            this.Channel = new Channel<TestController>(this.NetworkClientMock.Object, this.ProtocolBindingMock.Object.CreateParser(), this.ProtocolBindingMock.Object.MessageSerializer, this.ChannelControllerFactoryMock.Object);
            this.Channel.Notifier.ConnectionLost += (object sender, EventArgs e) => changed = true;
            this.ListenerMock.Raise(c => c.NetworkClientAccepted += null, new ClientAcceptedEventArgs(this.NetworkClientMock.Object));
            this.NetworkClientMock.Raise(c => c.ConnectionLost += null, EventArgs.Empty);
            changed.Should().BeTrue();
        }

        [TestMethod]
        public void Channel_should_notify_message_received()
        {
            bool changed = false;
            this.Channel = new Channel<TestController>(this.NetworkClientMock.Object, this.ProtocolBindingMock.Object.CreateParser(), this.ProtocolBindingMock.Object.MessageSerializer, this.ChannelControllerFactoryMock.Object);
            this.Channel.Notifier.ReceiveMessage += (object sender, IMessage e) => changed = true;
            this.MessageParserMock.Raise(c => c.MessageFound += null, new MessageFoundEventArgs(new Message()));
            changed.Should().BeTrue();
        }

        [TestMethod]
        public void Channel_should_notify_process_message_to_channel_controller()
        {
            this.Channel = new Channel<TestController>(this.NetworkClientMock.Object, this.ProtocolBindingMock.Object.CreateParser(), this.ProtocolBindingMock.Object.MessageSerializer, this.ChannelControllerFactoryMock.Object);
            this.ChannelControllerMock.Setup(c => c.ProcessMessage(It.IsAny<IChannelMessenger>())).Verifiable();
            this.Channel.Notifier.NotifyReceived(new Message());
            this.ChannelControllerMock.VerifyAll();
        }

        [TestMethod]
        public void Channel_should_send_message_to_client()
        {
            this.Channel = new Channel<TestController>(this.NetworkClientMock.Object, this.ProtocolBindingMock.Object.CreateParser(), this.ProtocolBindingMock.Object.MessageSerializer, this.ChannelControllerFactoryMock.Object);
            this.NetworkClientMock.Setup(c => c.SendSync(It.IsAny<byte[]>())).Verifiable();
            this.Channel.Notifier.NotifySend(new Message());
            this.NetworkClientMock.VerifyAll();
        }

        [TestMethod]
        public void Channel_should_put_data_to_parser()
        {
            this.Channel = new Channel<TestController>(this.NetworkClientMock.Object, this.ProtocolBindingMock.Object.CreateParser(), this.ProtocolBindingMock.Object.MessageSerializer, this.ChannelControllerFactoryMock.Object);
            this.MessageParserMock.Setup(c => c.Put(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>())).Verifiable();
            this.ListenerMock.Raise(c => c.NetworkClientAccepted += null, new ClientAcceptedEventArgs(this.NetworkClientMock.Object));
            this.NetworkClientMock.Raise(c => c.DataReceived += null, new DataReceivedEventArgs(0, new byte[] { }));
            this.MessageParserMock.VerifyAll();
        }

        [TestMethod]
        public void Channel_should_throw_argument_null_exception_when_network_client_is_null()
        {
            Action action = () => this.Channel = new Channel<TestController>(null, this.ProtocolBindingMock.Object.CreateParser(), this.ProtocolBindingMock.Object.MessageSerializer);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Channel_should_throw_argument_null_exception_when_message_serializer_is_null()
        {
            Action action = () => this.Channel = new Channel<TestController>(this.NetworkClientMock.Object, null, this.ProtocolBindingMock.Object.MessageSerializer);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Channel_should_throw_argument_null_exception_when_protocol_binding_is_null()
        {
            Action action = () => this.Channel = new Channel<TestController>(this.NetworkClientMock.Object, this.ProtocolBindingMock.Object.CreateParser(), null);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetworkBuffer.Communication.Messaging.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkBuffer.Tests.Communication.Messaging.Serialization
{
    /// <summary>
    /// Represents The default message parser tests.
    /// </summary>
    [TestClass]
    public class DefaultMessageParserTests
    {
        private DefaultMessageParser MessageParser { get; set; }
        private ParseSettings ParseSettings { get; set; }
        private Mock<IMessageSerializer> MessageSerializerMock { get; set; }
        private CancellationTokenSource TokenSource { get; set; }

        public DefaultMessageParserTests()
        {
            this.MessageSerializerMock = new Mock<IMessageSerializer>();
            this.TokenSource = new CancellationTokenSource();
            this.ParseSettings = new ParseSettings(this.MessageSerializerMock.Object);
            this.MessageParser = new DefaultMessageParser(this.ParseSettings, TokenSource.Token);
        }

        [TestMethod]
        public async Task MyTestMethod()
        {
            var buffer = new byte[] { 0x0A, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            await MessageParser.Put(buffer);
        }
    }
}
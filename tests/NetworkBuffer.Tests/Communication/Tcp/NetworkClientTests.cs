using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetworkBuffer.Communication.Tcp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NetworkBuffer.Tests.Communication.Tcp
{
    [TestClass]
    public class NetworkClientTests
    {
        private Mock<ITcpClient> TcpClientMock;
        private NetworkClient NetworkClient;
        private MemoryStream Stream;

        [TestInitialize]
        public void TestInitialize()
        {
            this.Stream = new MemoryStream();
            this.TcpClientMock = new Mock<ITcpClient>();
            this.TcpClientMock.Setup(c => c.Connected).Returns(true);
            this.TcpClientMock.Setup(c => c.Stream()).Returns(() => this.Stream);
            this.NetworkClient = new NetworkClient(TcpClientMock.Object);
        }

        [TestMethod]
        public async Task NetworkClient_should_invoke_DataReceived()
        {
            byte[] CutBuffer(byte[] _buffer, int offset, int size, int bufferSize)
            {
                byte[] cutBuffer = new byte[bufferSize];
                Array.Copy(_buffer, offset, cutBuffer, 0, size);
                return cutBuffer;
            }

            Random random = new Random();
            int dataBufferSize = 1500;
            int readBufferSize = 1024;
            byte[] buffer = new byte[dataBufferSize];

            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = (byte)random.Next(byte.MinValue, byte.MaxValue);

            this.Stream.Write(buffer, 0, buffer.Length);
            this.Stream.Position = 0;

            List<Tuple<byte[], int>> bufferList = new List<Tuple<byte[], int>>();
            this.NetworkClient.DataReceived += (object sender, DataReceivedEventArgs e) => { bufferList.Add(new Tuple<byte[], int>(e.Buffer, e.Size)); };
            await this.NetworkClient.Initialize();

            bufferList.Should().HaveCount(2);

            bufferList.ElementAt(0).Item1.Should().HaveCount(readBufferSize);
            bufferList.ElementAt(0).Item1.Should().Equal(CutBuffer(buffer, 0, readBufferSize, readBufferSize));
            bufferList.ElementAt(0).Item2.Should().Be(readBufferSize);

            bufferList.ElementAt(1).Item1.Should().Equal(CutBuffer(buffer, readBufferSize, dataBufferSize - readBufferSize, readBufferSize));
            bufferList.ElementAt(1).Item1.Should().HaveCount(readBufferSize);
            bufferList.ElementAt(1).Item2.Should().Be(dataBufferSize - readBufferSize);
        }

        [TestMethod]
        public async Task NetworkClient_should_invoke_ConnectionLost()
        {
            bool changed = false;
            this.NetworkClient.ConnectionLost += (object sender, EventArgs e) => { changed = true; };
            await this.NetworkClient.Initialize();
            changed.Should().BeTrue();
        }

        [TestMethod]
        public async Task NetworkClient_SendSync_should_write_to_stream()
        {
            Random random = new Random();
            int dataBufferSize = 1500;
            byte[] buffer = new byte[dataBufferSize];

            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = (byte)random.Next(byte.MinValue, byte.MaxValue);

            await this.NetworkClient.SendSync(buffer);

            this.Stream.Length.Should().Be(dataBufferSize);
        }
    }
}
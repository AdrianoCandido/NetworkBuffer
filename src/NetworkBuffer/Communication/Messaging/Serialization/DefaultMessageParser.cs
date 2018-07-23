using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkBuffer.Communication.Messaging.Serialization
{
    /// <summary>
    /// Represents The default message parser.
    /// </summary>
    /// <seealso cref="NetworkBuffer.Communication.Messaging.Serialization.IMessageParser" />
    public class DefaultMessageParser : IMessageParser, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultMessageParser" /> class.
        /// </summary>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public DefaultMessageParser(ParseSettings parseSettings, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.Settings = parseSettings ?? throw new ArgumentNullException(nameof(parseSettings));
            this.TokenSource = new CancellationTokenSource();
            this.Stream = new MemoryStream();
            this.Semaphore = new SemaphoreSlim(0);
            this.ProcessBufferAsync();
        }

        /// <summary>
        /// Gets the reset event slim.
        /// </summary>
        private SemaphoreSlim Semaphore { get; }

        /// <summary>
        /// Gets or sets the stream.
        /// </summary>
        private Stream Stream { get; set; }

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        public ParseSettings Settings { get; }

        /// <summary>
        /// Occurs when [message found].
        /// </summary>
        public event EventHandler<MessageFoundEventArgs> MessageFound;

        /// <summary>
        /// Occurs when [data discarded].
        /// </summary>
        public event EventHandler<byte[]> DataDiscarded;

        /// <summary>
        /// Gets the token source.
        /// </summary>
        private CancellationTokenSource TokenSource { get; }

        private int readPosition;

        /// <summary>
        /// Puts the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        public async Task Put(byte[] buffer)
        {
            await this.Put(buffer, buffer.Length, 0);
        }

        /// <summary>
        /// Puts the specified buffer to parser.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="size">The size.</param>
        /// <param name="index">The value.</param>
        public async Task Put(byte[] buffer, int size, int index = 0)
        {
            if (size > buffer.Length)
            {
                throw new InvalidOperationException("Size can not greater than buffer size.");
            }

            if (index > buffer.Length)
            {
                throw new IndexOutOfRangeException(nameof(index));
            }

            await this.Stream.WriteAsync(buffer, index, size);

            this.Stream.Position -= size;
            this.Semaphore.Release();
        }

        /// <summary>
        /// Processes the buffer.
        /// </summary>
        protected async void ProcessBufferAsync()
        {
            do
            {
                await this.Semaphore.WaitAsync(this.TokenSource.Token);

                int sizeBytes = 2;
                byte[] buffersize = new byte[sizeBytes];
                int bytesRead;
                bytesRead = await this.Stream.ReadAsync(buffersize, 0, buffersize.Length);

                int size = this.Settings.SizeInclusive ? sizeBytes : 0;

                if (this.Settings.IsBigEndian)
                {
                    Array.Reverse(buffersize);
                }

                size = BitConverter.ToInt16(buffersize, 0);

                byte[] buffer = new byte[size];

                bytesRead = 0;
                while (bytesRead < size)
                {
                    bytesRead = await this.Stream.ReadAsync(buffer, 0, buffer.Length);
                }
                if (this.Settings.Serializer.TryUnpack(buffer, size, 0, out IMessage message))
                {
                    this.MessageFound?.Invoke(this, new MessageFoundEventArgs(message));
                }
                else
                {
                    this.DataDiscarded?.Invoke(this, buffer);
                }
            }
            while (this.TokenSource.IsCancellationRequested == false);
        }

        public void Dispose()
        {
            this.TokenSource.Cancel();
        }
    }
}
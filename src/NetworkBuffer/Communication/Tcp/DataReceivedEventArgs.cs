using System;

namespace NetworkBuffer.Communication.Tcp
{
    /// <summary>
    /// Data received event
    /// </summary>
    public class DataReceivedEventArgs : EventArgs
    {
        #region Public Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DataReceivedEventArgs(int size, byte[] buffer)
        {
            this.Size = size;
            this.Buffer = buffer;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Buffer used to read client data.
        /// </summary>
        public byte[] Buffer { get; }

        /// <summary>
        /// Amount of data received.
        /// </summary>
        public int Size { get; }

        #endregion
    }
}
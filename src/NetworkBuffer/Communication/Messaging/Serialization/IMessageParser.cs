﻿using System;

namespace NetworkBuffer.Communication.Messaging.Serialization
{
    /// <summary>
    /// Represents the parser.
    /// </summary>
    public interface IMessageParser
    {
        /// <summary>
        /// Occurs when [message found].
        /// </summary>
        event EventHandler<MessageFoundEventArgs> MessageFound;

        /// <summary>
        /// Occurs when [data discarded].
        /// </summary>
        event EventHandler<byte[]> DataDiscarded;

        /// <summary>
        /// Puts the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="index">The value.</param>
        /// <param name="size">The size.</param>
        void PutData(byte[] buffer, int size, int index = 0);
    }
}
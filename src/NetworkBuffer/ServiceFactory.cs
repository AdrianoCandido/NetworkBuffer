﻿using NetworkBuffer.Channels;
using NetworkBuffer.Communication;
using NetworkBuffer.Messaging;
using System.Threading.Tasks;

namespace NetworkBuffer
{
    /// <summary>
    /// Factory class to channel.
    /// </summary>
    public static class ServiceFactory
    {
        #region Public Methods

        /// <summary>
        /// Create a new instance of the channel based on channel controller.
        /// </summary>
        /// <typeparam name="TController">type of logical controller to the channel.</typeparam>
        /// <param name="channelController">logical controller to the channel.</param>
        /// <returns>New instance of <see cref="HostService"/></returns>
        public static async Task<HostService<TController>> CreateService<TController>(TController channelController, MessagingProcessor messagingProcessor, Binding binding) where TController : class, IChannelController
        {
            return await new HostService<TController>(channelController, messagingProcessor, binding).StartAsync();
        }

        #endregion
    }
}
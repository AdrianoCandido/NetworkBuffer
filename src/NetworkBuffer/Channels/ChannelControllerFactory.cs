using System;

namespace NetworkBuffer.Channels
{
    /// <summary>
    /// Represents The channel controller factory.
    /// </summary>
    /// <typeparam name="TChannelController">The type of the channel controller.</typeparam>
    public class ChannelControllerFactory<TChannelController> where TChannelController : class, IChannelController
    {
        /// <summary>
        /// Crates the controller.
        /// </summary>
        /// <returns></returns>
        public virtual IChannelController CrateController()
        {
            return Activator.CreateInstance<TChannelController>();
        }
    }
}
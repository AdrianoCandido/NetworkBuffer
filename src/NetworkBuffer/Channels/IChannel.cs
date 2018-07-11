namespace NetworkBuffer.Channels
{
    /// <summary>
    /// Represents the channel.
    /// </summary>
    /// <typeparam name="TController">The type of the controller.</typeparam>
    public interface IChannel
    {
        /// <summary>
        /// Logical controller for the channel.
        /// </summary>
        IChannelController ChannelController { get; }
    }
}
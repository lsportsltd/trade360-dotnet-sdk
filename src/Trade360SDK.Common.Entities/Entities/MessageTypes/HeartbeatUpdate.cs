using Trade360SDK.Common.Attributes;

namespace Trade360SDK.Common.Entities.MessageTypes
{
    [Trade360Entity(32)]
    public class HeartbeatUpdate : MessageUpdate
    {
        /// <summary>
        /// Feed interruption signal. 0 = normal (default), non-zero = feed interrupted upstream.
        /// Signal only — does not trigger auto-suspend or recovery.
        /// </summary>
        public int FeedInterrupted { get; set; }
    }
}

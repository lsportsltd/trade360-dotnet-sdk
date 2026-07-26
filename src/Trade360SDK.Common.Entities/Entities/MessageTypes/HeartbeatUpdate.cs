using Trade360SDK.Common.Attributes;

namespace Trade360SDK.Common.Entities.MessageTypes
{
    [Trade360Entity(32)]
    public class HeartbeatUpdate : MessageUpdate
    {
        /// <summary>
        /// Feed health signal. 0 = no problem (default), non-zero = problem detected upstream.
        /// Signal only — does not trigger auto-suspend or recovery.
        /// </summary>
        public int Problem { get; set; }
    }
}

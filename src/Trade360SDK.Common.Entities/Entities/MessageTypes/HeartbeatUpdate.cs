using Trade360SDK.Common.Attributes;

namespace Trade360SDK.Common.Entities.MessageTypes
{
    [Trade360Entity(32)]
    public class HeartbeatUpdate : MessageUpdate
    {
        /// <summary>
        /// Feed interruption domains. Empty or absent = healthy.
        /// Phase 1: <c>[1]</c> = Markets (<see cref="FeedInterruptedDomainEnum.Markets"/>).
        /// Signal only — does not trigger auto-suspend or recovery.
        /// </summary>
        public int[] FeedInterrupted { get; set; } = System.Array.Empty<int>();
    }

    public enum FeedInterruptedDomainEnum
    {
        Markets = 1,
    }
}

﻿using Trade360SDK.Feed.Entities;

namespace Trade360SDK.Feed.Example.Handlers.Inplay
{
    internal class HeartbeatHandlerInplay : IEntityHandler<Heartbeat>
    {
        public Task ProcessAsync(Heartbeat entity)
        {
            Console.WriteLine("Heartbeat received");
            return Task.CompletedTask;
        }
    }
}

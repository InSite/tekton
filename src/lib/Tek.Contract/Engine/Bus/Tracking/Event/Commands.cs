using System;
using System.Collections.Generic;

using Tek.Contract;

namespace Tek.Contract.Engine
{
    public class CreateEvent
    {
        public Guid AggregateId { get; set; }
        public Guid EventId { get; set; }
        public Guid OriginId { get; set; }

        public string EventData { get; set; }
        public string EventType { get; set; }

        public int AggregateVersion { get; set; }
    }

    public class ModifyEvent
    {
        public Guid AggregateId { get; set; }
        public Guid EventId { get; set; }
        public Guid OriginId { get; set; }

        public string EventData { get; set; }
        public string EventType { get; set; }

        public int AggregateVersion { get; set; }
    }

    public class DeleteEvent
    {
        public Guid EventId { get; set; }
    }
}
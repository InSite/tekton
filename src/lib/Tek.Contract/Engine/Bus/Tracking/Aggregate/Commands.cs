using System;
using System.Collections.Generic;

using Tek.Contract;

namespace Tek.Contract.Engine
{
    public class CreateAggregate
    {
        public Guid AggregateId { get; set; }
        public Guid AggregateRoot { get; set; }

        public string AggregateType { get; set; }
    }

    public class ModifyAggregate
    {
        public Guid AggregateId { get; set; }
        public Guid AggregateRoot { get; set; }

        public string AggregateType { get; set; }
    }

    public class DeleteAggregate
    {
        public Guid AggregateId { get; set; }
    }

    public class ExportAggregates
    {
        public ICollection<CreateAggregate> Items { get; set; }
    }

    public class ImportAggregates
    {
        public ICollection<CreateAggregate> Items { get; set; }
    }

    public class PurgeAggregates
    {
        public ICollection<DeleteAggregate> Items { get; set; }
    }
}
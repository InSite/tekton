using System;
using System.Collections.Generic;

using Tek.Contract;

namespace Tek.Contract.Engine
{
    public class CreatePartition
    {
        public string PartitionEmail { get; set; }
        public string PartitionHost { get; set; }
        public string PartitionName { get; set; }
        public string PartitionSettings { get; set; }
        public string PartitionSlug { get; set; }
        public string PartitionTesters { get; set; }

        public int PartitionNumber { get; set; }

        public DateTime ModifiedWhen { get; set; }
    }

    public class ModifyPartition
    {
        public string PartitionEmail { get; set; }
        public string PartitionHost { get; set; }
        public string PartitionName { get; set; }
        public string PartitionSettings { get; set; }
        public string PartitionSlug { get; set; }
        public string PartitionTesters { get; set; }

        public int PartitionNumber { get; set; }

        public DateTime ModifiedWhen { get; set; }
    }

    public class DeletePartition
    {
        public int PartitionNumber { get; set; }
    }

    public class ExportPartitions
    {
        public ICollection<CreatePartition> Items { get; set; }
    }

    public class ImportPartitions
    {
        public ICollection<CreatePartition> Items { get; set; }
    }

    public class PurgePartitions
    {
        public ICollection<DeletePartition> Items { get; set; }
    }
}
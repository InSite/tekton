using System;
using System.Collections.Generic;

using Tek.Contract;

namespace Tek.Contract.Engine
{
    public class CreateOrigin
    {
        public Guid OrganizationId { get; set; }
        public Guid OriginId { get; set; }
        public Guid? ProxyAgent { get; set; }
        public Guid? ProxySubject { get; set; }
        public Guid UserId { get; set; }

        public string OriginDescription { get; set; }
        public string OriginReason { get; set; }
        public string OriginSource { get; set; }

        public DateTime OriginWhen { get; set; }
    }

    public class ModifyOrigin
    {
        public Guid OrganizationId { get; set; }
        public Guid OriginId { get; set; }
        public Guid? ProxyAgent { get; set; }
        public Guid? ProxySubject { get; set; }
        public Guid UserId { get; set; }

        public string OriginDescription { get; set; }
        public string OriginReason { get; set; }
        public string OriginSource { get; set; }

        public DateTime OriginWhen { get; set; }
    }

    public class DeleteOrigin
    {
        public Guid OriginId { get; set; }
    }

    public class ExportOrigins
    {
        public ICollection<CreateOrigin> Items { get; set; }
    }

    public class ImportOrigins
    {
        public ICollection<CreateOrigin> Items { get; set; }
    }

    public class PurgeOrigins
    {
        public ICollection<DeleteOrigin> Items { get; set; }
    }
}
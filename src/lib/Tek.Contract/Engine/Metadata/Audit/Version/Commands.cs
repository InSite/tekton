using System;
using System.Collections.Generic;

using Tek.Contract;

namespace Tek.Contract.Engine
{
    public class CreateVersion
    {
        public string ScriptContent { get; set; }
        public string ScriptPath { get; set; }
        public string VersionName { get; set; }
        public string VersionType { get; set; }

        public int VersionNumber { get; set; }

        public DateTime ScriptExecuted { get; set; }
    }

    public class ModifyVersion
    {
        public string ScriptContent { get; set; }
        public string ScriptPath { get; set; }
        public string VersionName { get; set; }
        public string VersionType { get; set; }

        public int VersionNumber { get; set; }

        public DateTime ScriptExecuted { get; set; }
    }

    public class DeleteVersion
    {
        public int VersionNumber { get; set; }
    }

    public class ExportVersions
    {
        public ICollection<CreateVersion> Items { get; set; }
    }

    public class ImportVersions
    {
        public ICollection<CreateVersion> Items { get; set; }
    }

    public class PurgeVersions
    {
        public ICollection<DeleteVersion> Items { get; set; }
    }
}
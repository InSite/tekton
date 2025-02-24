using System;
using System.Collections.Generic;

using Tek.Contract;

namespace Tek.Contract.Engine
{
    public class CreateRole
    {
        public Guid RoleId { get; set; }

        public string RoleName { get; set; }
        public string RoleType { get; set; }
    }

    public class ModifyRole
    {
        public Guid RoleId { get; set; }

        public string RoleName { get; set; }
        public string RoleType { get; set; }
    }

    public class DeleteRole
    {
        public Guid RoleId { get; set; }
    }

    public class ExportRoles
    {
        public ICollection<CreateRole> Items { get; set; }
    }

    public class ImportRoles
    {
        public ICollection<CreateRole> Items { get; set; }
    }

    public class PurgeRoles
    {
        public ICollection<DeleteRole> Items { get; set; }
    }
}
using System;
using System.Collections.Generic;

using Tek.Contract;

namespace Tek.Contract.Engine
{
    public class CreatePermission
    {
        public Guid PermissionId { get; set; }
        public Guid ResourceId { get; set; }
        public Guid RoleId { get; set; }

        public string AccessType { get; set; }

        public int AccessFlags { get; set; }
    }

    public class ModifyPermission
    {
        public Guid PermissionId { get; set; }
        public Guid ResourceId { get; set; }
        public Guid RoleId { get; set; }

        public string AccessType { get; set; }

        public int AccessFlags { get; set; }
    }

    public class DeletePermission
    {
        public Guid PermissionId { get; set; }
    }

    public class ExportPermissions
    {
        public ICollection<CreatePermission> Items { get; set; }
    }

    public class ImportPermissions
    {
        public ICollection<CreatePermission> Items { get; set; }
    }

    public class PurgePermissions
    {
        public ICollection<DeletePermission> Items { get; set; }
    }
}
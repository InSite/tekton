using System;
using System.Collections.Generic;

using Tek.Contract;

namespace Tek.Contract.Engine
{
    public class CreateProvince
    {
        public Guid? CountryId { get; set; }
        public Guid ProvinceId { get; set; }

        public string CountryCode { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
    }

    public class ModifyProvince
    {
        public Guid? CountryId { get; set; }
        public Guid ProvinceId { get; set; }

        public string CountryCode { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
    }

    public class DeleteProvince
    {
        public Guid ProvinceId { get; set; }
    }

    public class ExportProvinces
    {
        public ICollection<CreateProvince> Items { get; set; }
    }

    public class ImportProvinces
    {
        public ICollection<CreateProvince> Items { get; set; }
    }

    public class PurgeProvinces
    {
        public ICollection<DeleteProvince> Items { get; set; }
    }
}
using System;
using System.Collections.Generic;

using Tek.Contract;

namespace Tek.Contract.Engine
{
    public class CreateCountry
    {
        public Guid CountryId { get; set; }

        public string CapitalCityName { get; set; }
        public string ContinentCode { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string Languages { get; set; }
        public string TopLevelDomain { get; set; }
    }

    public class ModifyCountry
    {
        public Guid CountryId { get; set; }

        public string CapitalCityName { get; set; }
        public string ContinentCode { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string Languages { get; set; }
        public string TopLevelDomain { get; set; }
    }

    public class DeleteCountry
    {
        public Guid CountryId { get; set; }
    }

    public class ExportCountries
    {
        public ICollection<CreateCountry> Items { get; set; }
    }

    public class ImportCountries
    {
        public ICollection<CreateCountry> Items { get; set; }
    }

    public class PurgeCountries
    {
        public ICollection<DeleteCountry> Items { get; set; }
    }
}
namespace Tek.Contract.Engine
{
    public static partial class Endpoints
    {
        public static partial class BusApi
        {
            public static partial class Tracking
            {
                public static class Aggregate
                {
                    public const string Collection = "aggregates";
                    public const string Item = "/{aggregate:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                    
                    public const string Export = Collection + "/export";  // POST : Download multiple items
                    public const string Import = Collection + "/import";  // POST : Upload multiple items
                    public const string Purge  = Collection + "/purge";   // POST : Delete multiple items
                }

                public static class Event
                {
                    public const string Collection = "events";
                    public const string Item = "/{event:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                    
                    public const string Export = Collection + "/export";  // POST : Download multiple items
                    public const string Import = Collection + "/import";  // POST : Upload multiple items
                    public const string Purge  = Collection + "/purge";   // POST : Delete multiple items
                }
            }
        }

        public static partial class ContactApi
        {
            public static partial class Location
            {
                public static class Country
                {
                    public const string Collection = "countrys";
                    public const string Item = "/{country:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                    
                    public const string Export = Collection + "/export";  // POST : Download multiple items
                    public const string Import = Collection + "/import";  // POST : Upload multiple items
                    public const string Purge  = Collection + "/purge";   // POST : Delete multiple items
                }

                public static class Province
                {
                    public const string Collection = "provinces";
                    public const string Item = "/{province:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                    
                    public const string Export = Collection + "/export";  // POST : Download multiple items
                    public const string Import = Collection + "/import";  // POST : Upload multiple items
                    public const string Purge  = Collection + "/purge";   // POST : Delete multiple items
                }
            }
        }

        public static partial class ContentApi
        {
            public static partial class Text
            {
                public static class Translation
                {
                    public const string Collection = "translations";
                    public const string Item = "/{translation:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                    
                    public const string Export = Collection + "/export";  // POST : Download multiple items
                    public const string Import = Collection + "/import";  // POST : Upload multiple items
                    public const string Purge  = Collection + "/purge";   // POST : Delete multiple items
                }
            }
        }

        public static partial class MetadataApi
        {
            public static partial class Audit
            {
                public static class Origin
                {
                    public const string Collection = "origins";
                    public const string Item = "/{origin:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                    
                    public const string Export = Collection + "/export";  // POST : Download multiple items
                    public const string Import = Collection + "/import";  // POST : Upload multiple items
                    public const string Purge  = Collection + "/purge";   // POST : Delete multiple items
                }

                public static class Version
                {
                    public const string Collection = "versions";
                    public const string Item = "/{version:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                    
                    public const string Export = Collection + "/export";  // POST : Download multiple items
                    public const string Import = Collection + "/import";  // POST : Upload multiple items
                    public const string Purge  = Collection + "/purge";   // POST : Delete multiple items
                }
            }
        }

        public static partial class SecurityApi
        {
            public static partial class Authorization
            {
                public static class Permission
                {
                    public const string Collection = "permissions";
                    public const string Item = "/{permission:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                    
                    public const string Export = Collection + "/export";  // POST : Download multiple items
                    public const string Import = Collection + "/import";  // POST : Upload multiple items
                    public const string Purge  = Collection + "/purge";   // POST : Delete multiple items
                }

                public static class Resource
                {
                    public const string Collection = "resources";
                    public const string Item = "/{resource:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                    
                    public const string Export = Collection + "/export";  // POST : Download multiple items
                    public const string Import = Collection + "/import";  // POST : Upload multiple items
                    public const string Purge  = Collection + "/purge";   // POST : Delete multiple items
                }
            }

            public static partial class Identification
            {
                public static class Organization
                {
                    public const string Collection = "organizations";
                    public const string Item = "/{organization:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                    
                    public const string Export = Collection + "/export";  // POST : Download multiple items
                    public const string Import = Collection + "/import";  // POST : Upload multiple items
                    public const string Purge  = Collection + "/purge";   // POST : Delete multiple items
                }

                public static class Partition
                {
                    public const string Collection = "partitions";
                    public const string Item = "/{partition:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                    
                    public const string Export = Collection + "/export";  // POST : Download multiple items
                    public const string Import = Collection + "/import";  // POST : Upload multiple items
                    public const string Purge  = Collection + "/purge";   // POST : Delete multiple items
                }

                public static class Password
                {
                    public const string Collection = "passwords";
                    public const string Item = "/{password:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                    
                    public const string Export = Collection + "/export";  // POST : Download multiple items
                    public const string Import = Collection + "/import";  // POST : Upload multiple items
                    public const string Purge  = Collection + "/purge";   // POST : Delete multiple items
                }

                public static class Role
                {
                    public const string Collection = "roles";
                    public const string Item = "/{role:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                    
                    public const string Export = Collection + "/export";  // POST : Download multiple items
                    public const string Import = Collection + "/import";  // POST : Upload multiple items
                    public const string Purge  = Collection + "/purge";   // POST : Delete multiple items
                }
            }
        }

    }
}
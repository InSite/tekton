namespace Tek.Contract
{
    public static partial class Endpoints
    {
        public const string Token = "api/token";
        public const string Validate = "api/token/validate";
        public const string Status = "api/status";
        public const string Version = "api/version";

        public static class Debug
        {
            public const string Endpoints = "api/debug/endpoints";
            public const string Permissions = "api/debug/permissions";
            public const string Resources = "api/debug/resources";
            public const string Token = "api/debug/token";
        }

        public static class React
        {
            public const string Queries = "api/debug/endpoints";
        }

        public static class Location
        {
            public const string Countries = "api/debug/endpoints";
        }
    }
}
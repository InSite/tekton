namespace Tek.Api.Engine
{
    public static class HttpResponseExtensions
    {
        public static void AddPagination(this HttpResponse response, Pagination pagination)
        {
            response.Headers.Append(Pagination.HeaderKey, System.Text.Json.JsonSerializer.Serialize(pagination));
        }

        public static void AddPagination(this HttpResponse response, int? page, int? take, int items, int total)
            => response.AddPagination(new Pagination(page, take, items, total));
    }
}

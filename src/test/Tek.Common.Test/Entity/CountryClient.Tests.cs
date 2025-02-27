using Tek.Toolbox;

namespace Tek.Common.Test;

[Trait("Category", "SDK")]
public class CountryClientTests
{
    [Fact]
    public async Task Collect_TakeFive_NoExceptionThrown()
    {

        var client = new CountryClient();

        var query = new CollectCountries() { Filter = new Filter { Take = 5 } };

        var host = "https://localhost:5000";

        var secret = "The secret key assigned to Root sentinel goes here.";

        var factory = new HttpClientFactory(new Uri(host), null);

        var serializer = new JsonSerializer();

        var api = new ApiClient(factory, serializer);

        var response = await api.HttpPost<string>("token", new { Secret = secret });

        factory = new HttpClientFactory(new Uri(host), response.Data);

        api = new ApiClient(factory, serializer);

        var matches = await client.CollectAsync(api, query);
    }
}
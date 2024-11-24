using Services.Models;
using System.Text.Json;

public class IexStockService : IStockService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public IexStockService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<Stock>?> GetStocksAsync()
    {
        using var client = _httpClientFactory.CreateClient();
        var response = await client.GetStringAsync("https://api.iexcloud.io/stable/stock/market/list/mostactive?token=YOUR_API_KEY");
        var stocks = JsonSerializer.Deserialize<List<Stock>>(response);
        return stocks;
    }
}

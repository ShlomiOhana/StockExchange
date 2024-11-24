using Services.Models;
using System.Text.Json;

public class AlphaStockService : IStockService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public AlphaStockService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<List<Stock>> GetStocksAsync()
    {
        using var client = _httpClientFactory.CreateClient();

        var symbols = "AAPL,MSFT,TSLA";
        var apiKey = _configuration["ApiKey"];
        var url = $"https://alphavantage.co/query?function=TIME_SERIES_DAILY_ADJUSTED&symbol={symbols}&apikey={apiKey}&datatype=json";
        var response = await client.GetStringAsync(url);
        var data = JsonSerializer.Deserialize<JsonElement>(response);
        var stocksList = new List<Stock>();

        // check if data exists or not

        return stocksList;
    }
}

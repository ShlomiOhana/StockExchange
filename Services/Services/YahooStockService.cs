using Services.Models;
using System.Text.Json;

public class YahooStockService : IStockService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public YahooStockService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<Stock>> GetStocksAsync()
    {
        using var client = _httpClientFactory.CreateClient();

        var symbols = "AAPL,MSFT,TSLA";
        var url = $"https://query1.finance.yahoo.com/v7/finance/quote?symbols={symbols}";
        var response = await client.GetStringAsync(url);
        var data = JsonSerializer.Deserialize<JsonElement>(response);
        var stocksList = new List<Stock>();

        foreach (var stock in data.GetProperty("quoteResponse").GetProperty("result").EnumerateArray())
        {
            var stockItem = new Stock
            {
                Symbol = stock.GetProperty("symbol").GetString() ?? "",
                CompanyName = stock.GetProperty("shortName").GetString() ?? "",
                CurrentPrice = stock.GetProperty("regularMarketPrice").GetDecimal(),
                ChangePercentage = stock.GetProperty("regularMarketChangePercent").GetDecimal(),
            };

            stocksList.Add(stockItem);
        }

        return stocksList;
    }
}

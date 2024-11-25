using Services.Models;
using YahooFinanceApi;

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
        var response = await Yahoo.Symbols(symbols).Fields(Field.Symbol, Field.RegularMarketPrice, Field.LongName, Field.RegularMarketPreviousClose).QueryAsync();
        var stocksList = new List<Stock>();

        foreach (var stock in response)
        {
            var currentPrice = (decimal)stock.Value.Fields["RegularMarketPrice"];
            var prevPrice = (decimal)stock.Value.Fields["RegularMarketPreviousClose"];
            var companyName = stock.Value.Fields["LongName"] ?? "";
            var symbol = stock.Key ?? "";

            var priceDiff = currentPrice - prevPrice;
            var PrecentageDiff = priceDiff / prevPrice;

            var stockItem = new Stock
            {
                Symbol = symbol,
                CompanyName = companyName,
                CurrentPrice = currentPrice,
                ChangePercentage = Decimal.Round(PrecentageDiff, 2, MidpointRounding.AwayFromZero),
            };

            stocksList.Add(stockItem);
        }

        return stocksList;
    }
}

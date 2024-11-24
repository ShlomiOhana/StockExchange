using AppServices.Services;
using Microsoft.AspNetCore.Mvc;
using Services.Models;

[ApiController]
[Route("api/stock")]
public class StocksController : ControllerBase
{
    private readonly IStockService _stockService;
    private readonly MemoryCacheService _memoryCacheService;

    public StocksController(IStockService stockService, MemoryCacheService memoryCacheService)
    {
        _stockService = stockService;
        _memoryCacheService = memoryCacheService;
    }

    [HttpGet]
    public async Task<IActionResult> GetStocks()
    {
        try
        {
            var stocks = await _stockService.GetStocksAsync();
            if (stocks != null)
            {
                _memoryCacheService.SetStocks(stocks);
            }
            else
            {
                stocks = _memoryCacheService.GetStocks();
            }

            stocks = new List<Stock>
            {
                new Stock { Symbol = "AAPL", CompanyName = "Apple Inc.", CurrentPrice = 150.25m, ChangePercentage = 1.69m },
                new Stock { Symbol = "GOOGL", CompanyName = "Alphabet Inc.", CurrentPrice = 2800.75m, ChangePercentage = -0.54m },
                new Stock { Symbol = "AMZN", CompanyName = "Amazon.com Inc.", CurrentPrice = 3405.70m, ChangePercentage = 0.30m },
                new Stock { Symbol = "MSFT", CompanyName = "Microsoft Corporation", CurrentPrice = 299.60m, ChangePercentage = 1.28m },
                new Stock { Symbol = "TSLA", CompanyName = "Tesla Inc.", CurrentPrice = 715.90m, ChangePercentage = 1.77m },
                new Stock { Symbol = "META", CompanyName = "Meta Platforms Inc.", CurrentPrice = 330.50m, ChangePercentage = -1.55m },
                new Stock { Symbol = "NFLX", CompanyName = "Netflix Inc.", CurrentPrice = 513.50m, ChangePercentage = 1.73m }
            };

            return Ok(stocks);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Failed to fetch stocks", details = ex.Message });
        }
    }
}

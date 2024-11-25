using AppServices.Services;
using Microsoft.AspNetCore.Mvc;

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

            return Ok(stocks);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Failed to fetch stocks", details = ex.Message });
        }
    }
}

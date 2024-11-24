using Services.Models;

public interface IStockService
{
    Task<List<Stock>> GetStocksAsync();
}

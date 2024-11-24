using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Caching.Memory;
using Services.Models;

public class GoogleStockService : IStockService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IHttpClientFactory _httpClientFactory;
    static string[] Scopes = { SheetsService.Scope.Spreadsheets };
    static string ApplicationName = "Google Sheets API .NET Quickstart";
    static string SpreadsheetId = "your-spreadsheet-id"; // Replace with your Google Sheet ID
    static string Range = "Sheet1!A1:B10"; // Adjust the range to your needs

    public GoogleStockService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<Stock>?> GetStocksAsync()
    {
        var credential = await GetCredentialsAsync();
        var service = new SheetsService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });

        // Call the Sheets API to get the values
        var request = service.Spreadsheets.Values.Get(SpreadsheetId, Range);
        var response = await request.ExecuteAsync();

        // Output the values
        Console.WriteLine("Data from Google Sheet:");
        foreach (var row in response.Values)
        {
            // Print each row
            Console.WriteLine(string.Join(", ", row));
        }

        return null;
    }

    private async Task<UserCredential> GetCredentialsAsync()
    {
        // Path to the credentials JSON file
        string credPath = "token.json";

        using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
        {
            // Authenticate using OAuth2
            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true));
            return credential;
        }
    }
}

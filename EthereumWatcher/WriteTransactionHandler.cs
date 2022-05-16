using System.Net.Http.Json;

namespace EthereumWatcher;

public class WriteTransactionHandler
{
    static HttpClient client = new HttpClient();

    public static async Task Handle(BlockchainWriteTransaction tx)
    {
        var response = await client.PostAsJsonAsync("http://localhost:5254/alert", tx);
        response.EnsureSuccessStatusCode();
    }
}

public record BlockchainWriteTransaction(string Contract, string FunctionName, string TxHash, DateTime TimestampUtc);
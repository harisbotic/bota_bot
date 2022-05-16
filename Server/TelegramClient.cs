public class TelegramClient
{
    private readonly HttpClient _httpClient;

    public TelegramClient(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;

        _httpClient.BaseAddress = new Uri($"https://api.telegram.org/bot{config["BOT_TOKEN"]}/");
    }

    public async Task SendMessage(string chatId, string text)
    {
        var response = await _httpClient.GetAsync($"sendMessage?chat_id={chatId}&text={text}&parse_mode=html&disable_web_page_preview=true");
        response.EnsureSuccessStatusCode();
    }
}
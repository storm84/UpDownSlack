using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using UpDown.Worker.Clients.Models;
using UpDown.Worker.Configurations;

namespace UpDown.Worker.Clients;

public interface ISlackClient
{
    Task SendAsync(string message);
    Task SendAsync(string channel, string message);
}

public class SlackClient : ISlackClient
{
    public HttpClient Client { get; }
    public SlackConfig SlackConfig { get; }

    public SlackClient(HttpClient client, IOptions<SlackConfig> slackConfig)
    {
        SlackConfig = slackConfig.Value ?? throw new InvalidOperationException("Missing slack configuration");
        Client = client;
        Client.BaseAddress = new Uri("https://slack.com");
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SlackConfig.Token);
    }

    public Task SendAsync(string message)
    {
        return SendAsync(message, SlackConfig.DefaultChannel);
    }

    public async Task SendAsync(string message, string channel)
    {
        var res = await Client.PostAsJsonAsync(
            "api/chat.postMessage",
            new PostMessageRequest(message, channel));
    }
}
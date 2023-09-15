using Microsoft.Extensions.Options;
using UpDown.Worker.Clients;
using UpDown.Worker.Configurations;
using UpDown.Worker.Models;

namespace UpDown.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    public ISlackClient SlackClient { get; }
    public UpDownConfig UpDownConfig { get; }
    public IPingService PingService { get; }

    public Worker(
        ILogger<Worker> logger,
        ISlackClient slackClient,
        IOptions<UpDownConfig> upDownConfig,
        IPingService pingService)
    {
        _logger = logger;
        SlackClient = slackClient;
        PingService = pingService;
        UpDownConfig = upDownConfig.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            var tasks = UpDownConfig.Services.Select(PingAndReport).ToList();
            await Task.WhenAll(tasks);

            await Task.Delay(TimeSpan.FromSeconds(UpDownConfig.IntervalSeconds), stoppingToken);
        }
    }

    private async Task PingAndReport(Service service)
    {
        var res = await PingService.Ping(service, UpDownConfig.SlowThresholdMs);

        if (res.Status == UpDownStatus.Up && UpDownConfig.ReportSuccess)
        {
            await SlackClient.SendAsync($"🟢 {res.Service} is up, response time {res.ResponseTime}ms");
        }
        else if (res.Status == UpDownStatus.Slow && UpDownConfig.ReportSlow)
        {
            await SlackClient.SendAsync($"🟡 {res.Service} is slow, response time {res.ResponseTime}ms");
        }
        else if (res.Status == UpDownStatus.Down)
        {
            await SlackClient.SendAsync($"🔴 {res.Service} is down, reason: {res.Reason}");
        }
    }
}
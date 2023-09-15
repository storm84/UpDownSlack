using System.Diagnostics;
using UpDown.Worker.Configurations;

namespace UpDown.Worker;

public interface IPingService
{
    Task<UpDownResponse> Ping(Service service, long defaultSlowThreshold);
}

public class PingService : IPingService
{
    public HttpClient Client { get; }

    public PingService(HttpClient client)
    {
        Client = client;
    }

    public async Task<UpDownResponse> Ping(Service service, long defaultSlowThreshold)
    {
        HttpResponseMessage? res = null;
        var request = new HttpRequestMessage
        {
            RequestUri = new Uri(service.Url),
            Method = service.RequestType switch
            {
                RequestType.HEAD => HttpMethod.Head,
                RequestType.GET => HttpMethod.Get,
                RequestType.POST => HttpMethod.Post,
                _ => throw new InvalidOperationException("Invalid RequestType")
            }
        };
        var sw = Stopwatch.StartNew();
        try
        {
            res = await Client.SendAsync(request);
        }
        catch (HttpRequestException)
        {
            //reason = ex.Message;
        }

        sw.Stop();

        return UpDownResponse.Create(
            service,
            res,
            sw.ElapsedMilliseconds,
            sw.ElapsedMilliseconds > (service.SlowThresholdMsOverride ?? defaultSlowThreshold));
    }
}
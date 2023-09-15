using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net;
using UpDown.Worker.Configurations;
using UpDown.Worker.Models;

namespace UpDown.Worker;

public class UpDownResponse
{
    public string Service { get; }
    public string Url { get; }
    public UpDownStatus Status { get; }
    public long ResponseTime { get; }
    public string? Reason { get; }

    private UpDownResponse(
        string service,
        string url,
        UpDownStatus status,
        long responseTime,
        string? reason)
    {
        Service = service;
        Url = url;
        Status = status;
        ResponseTime = responseTime;
        Reason = reason;
    }

    public static UpDownResponse Create(
        Service service,
        HttpResponseMessage? responseMessage,
        long responseTime,
        bool isSlow)
    {
        return new UpDownResponse(
            service.Name,
            service.Url,
            MapStatus(responseMessage, isSlow),
            responseTime,
            MapReason(responseMessage));
    }

    private static string? MapReason(HttpResponseMessage? responseMessage)
    {
        if (responseMessage is null)
        {
            return "Error trying to call the service";
        }

        return responseMessage.ReasonPhrase;
    }

    private static UpDownStatus MapStatus(HttpResponseMessage? responseMessage, bool isSlow)
    {
        if (responseMessage is null || !responseMessage.IsSuccessStatusCode)
        {
            return UpDownStatus.Down;
        }

        return isSlow ? UpDownStatus.Slow : UpDownStatus.Up;
    }
}
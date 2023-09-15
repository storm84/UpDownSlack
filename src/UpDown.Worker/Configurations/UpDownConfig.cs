namespace UpDown.Worker.Configurations;

public class UpDownConfig
{
    public static string ConfigurationSection => nameof(UpDownConfig);
    public int IntervalSeconds { get; init; } = 30;
    public bool ReportSuccess { get; init; } = false;
    public bool ReportSlow { get; set; } = false;
    public long SlowThresholdMs { get; set; } = 500;
    public IEnumerable<Service> Services { get; set; } = new List<Service>();
}

public class Service
{
    public string Name { get; init; } = null!;
    public string Url { get; init; } = null!;
    public RequestType RequestType { get; init; }
    public long? SlowThresholdMsOverride { get; init; } = null!;
}
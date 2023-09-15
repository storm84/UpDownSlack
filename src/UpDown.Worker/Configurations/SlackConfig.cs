namespace UpDown.Worker.Configurations;

public class SlackConfig
{
    public static string ConfigurationSection => nameof(SlackConfig);
    public string Token { get; init; } = null!;
    public string DefaultChannel { get; init; } = null!;
}
namespace UpDown.Worker.Clients.Models;

public class PostMessageRequest
{
    public string Text { get; }
    public string Channel { get; }

    public PostMessageRequest(string text, string channel)
    {
        Text = text;
        Channel = channel;
    }
}
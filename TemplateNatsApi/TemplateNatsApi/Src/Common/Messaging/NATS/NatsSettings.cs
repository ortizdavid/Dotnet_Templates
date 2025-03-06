namespace TemplateNatsApi.Common.Messaging.NATS;

public class NatsSettings
{
    public string Url { get; set; } = "nats://localhost:4222";
    public string User { get; set; } = "";
    public string Password { get; set; } = "";
    public int Timeout { get; set; } = 5000;
    public int ReconnectWait { get; set; } = 2000;
    public int MaxReconnects { get; set; } = 5;
}
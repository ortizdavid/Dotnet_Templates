namespace TemplateEventDriven.Common.Messaging.RabbitMQ;

public class Queue
{
    public string? Name { get; set; }
    public bool Durable { get; set; } = true;
    public bool Exclusive { get; set; } = false;
    public bool AutoDelete { get; set; } = false;
}

public class Exchange
{
    public string? Name { get; set; }
    public ExchangeType Type { get; set; }
    public bool Durable { get; set; } = true;
    public bool AutoDelete { get; set; } = false;
}

public enum ExchangeType
{
    Fanout,
    Direct,
    Topic,
    Headers
}

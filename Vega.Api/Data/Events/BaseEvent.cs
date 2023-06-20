namespace Vega.Api.Data.Events;

public class BaseEvent
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
}

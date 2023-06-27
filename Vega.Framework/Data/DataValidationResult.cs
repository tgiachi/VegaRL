namespace Vega.Framework.Data;

public class DataValidationResult
{
    public string EntityType { get; set; }
    public string PropertyName { get; set; }
    public string Message { get; set; }

    public override string ToString() =>
        $" {nameof(EntityType)}: {EntityType}, {nameof(PropertyName)}: {PropertyName}, {nameof(Message)}: {Message}";
}

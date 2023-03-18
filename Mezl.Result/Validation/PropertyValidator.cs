namespace Mezl.Result.Validation;

public class PropertyValidator<T>
{
    private readonly ValidationContext _validationContext;

    internal string PropertyName { get; }
    internal T Value { get; }

    public PropertyValidator(string propertyName, ValidationContext validationContext, T value)
    {
        _validationContext = validationContext;
        PropertyName = propertyName;
        Value = value;
    }

    public void Add(string error)
    {
        _validationContext.Add(PropertyName, error);
    }
}
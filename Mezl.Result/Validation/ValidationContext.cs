using System.Runtime.CompilerServices;
using Mezl.Result.Reasons;

namespace Mezl.Result.Validation;

public class ValidationContext
{

    private Dictionary<string, List<string>> _dictionary;

    public IReadOnlyDictionary<string, IReadOnlyCollection<string>> Errors => _dictionary as IReadOnlyDictionary<string, IReadOnlyCollection<string>>;

    public PropertyValidator<T> Property<T>(T value, [CallerArgumentExpression("value")] string propertyName = default)
    {
        return new PropertyValidator<T>(propertyName, this, value);
    }

    internal void Add(string propertyName, string error)
    {
        if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));
        if (error == null) throw new ArgumentNullException(nameof(error));

        _dictionary ??= new Dictionary<string, List<string>>();

        if (_dictionary.TryGetValue(propertyName, out var errors))
        {
            errors.Add(error);
        }
        else
        {
            _dictionary.Add(propertyName, new List<string> { error });
        }
    }

    public R GetValidationResult()
    {
        if (_dictionary != null && _dictionary.Any())
        {
            return Reason.New<ReasonValidationFailed>().AddInit(_dictionary);
        }

        return R.Success;
    }
}
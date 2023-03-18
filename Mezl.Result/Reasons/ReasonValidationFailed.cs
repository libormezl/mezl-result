using System.Runtime.CompilerServices;

namespace Mezl.Result.Reasons
{
    public class PropertyValidator<T>
    {
        // todo: maybe to internal?? ALLL
        private readonly ValidationContext _validationContext;
        public string PropertyName { get; }
        public T Value { get; }

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

    public class ValidationContext
    {
        private ValidationContext() { }

        public static ValidationContext New()
        {
            return new ValidationContext();
        }

        private Dictionary<string, List<string>> _dictionary;

        public IReadOnlyDictionary<string, IReadOnlyCollection<string>> Errors => _dictionary as IReadOnlyDictionary<string, IReadOnlyCollection<string>>;

        public PropertyValidator<T> Property<T>(T value, [CallerArgumentExpressionAttribute("value")] string propertyName = default)
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
            if (_dictionary.Any())
            {
                return Reason.New<ReasonValidationFailed>().AddInit(_dictionary);
            }

            return R.Success;
        }
    }

    public class ReasonValidationFailed : Reason
    {
        private Dictionary<string, List<string>> _dictionary;

        public IReadOnlyDictionary<string, IReadOnlyCollection<string>> PropertyErrors => _dictionary as IReadOnlyDictionary<string, IReadOnlyCollection<string>>;

        internal ReasonValidationFailed AddInit(Dictionary<string, List<string>> dictionary)
        {
            _dictionary = dictionary;
            return this;
        }
    }
}

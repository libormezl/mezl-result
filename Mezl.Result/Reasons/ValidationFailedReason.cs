namespace Mezl.Result.Reasons
{
    public class ValidationFailedReason : Reason
    {
        private Dictionary<string, List<string>> _dictionary;

        public IReadOnlyDictionary<string, IReadOnlyCollection<string>> PropertyErrors => _dictionary as IReadOnlyDictionary<string, IReadOnlyCollection<string>>;

        public ValidationFailedReason Add(string propertyName, string error)
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

            return this;
        }

        public bool ValidationFailed => _dictionary is not null;
    }
}

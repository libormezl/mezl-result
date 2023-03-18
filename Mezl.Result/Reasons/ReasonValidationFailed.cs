namespace Mezl.Result.Reasons
{
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

namespace Mezl.Result.Reasons;

public class ReasonValidationFailed : Reason
{
    private Dictionary<string, IReadOnlyCollection<string>> _dictionary;

    public IReadOnlyDictionary<string, IReadOnlyCollection<string>> PropertyErrors => _dictionary;

    internal ReasonValidationFailed Init(Dictionary<string, IReadOnlyCollection<string>> dictionary)
    {
        _dictionary = dictionary;
        return this;
    }
}
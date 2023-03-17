namespace Mezl.Result.Reasons;

public class AggregatedReason : Reason
{
    public readonly IReadOnlyCollection<Reason> Reasons;

    public AggregatedReason(IReadOnlyCollection<Reason> reasons)
    {
        Reasons = reasons;
    }
}
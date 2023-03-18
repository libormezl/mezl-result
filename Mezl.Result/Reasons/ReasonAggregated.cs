namespace Mezl.Result.Reasons;

public class ReasonAggregated : Reason
{
    public readonly IReadOnlyCollection<Reason> Reasons;

    public ReasonAggregated(params Reason[] reasons)
    {
        Reasons = reasons;
    }
}
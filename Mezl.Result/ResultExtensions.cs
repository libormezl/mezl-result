namespace Mezl.Result;

public static class ResultExtensions
{
    public static bool Is<T>(this R result) where T : Reason
    {
        return result.Reason is T;
    }

    public static bool IsNot<T>(this R result) where T : Reason
    {
        return result.Reason is not T;
    }
}
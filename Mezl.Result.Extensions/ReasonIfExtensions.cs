namespace Mezl.Result.Extensions;

public static class ReasonIfExtensions
{
    public static async Task<R<TRValue>> ReasonIf<TRValue>(this Task<R<TRValue>> resultTask, Func<TRValue, bool> condition, Reason reason)
    {
        var result = await resultTask;
        if (result.IsNotSuccessful)
        {
            return result.Reason;
        }

        if (condition(result.Value))
        {
            return reason;
        }

        return result.Value;
    }

    public static async Task<R<TRValue>> ReasonIfAsync<TRValue>(this Task<R<TRValue>> resultTask, Func<TRValue, Task<bool>> condition, Reason reason)
    {
        var result = await resultTask;
        if (result.IsNotSuccessful)
        {
            return result.Reason;
        }

        if (await condition(result.Value))
        {
            return reason;
        }

        return result.Value;
    }

    public static R<TRValue> ReasonIf<TRValue>(this R<TRValue> result, Func<TRValue, bool> condition, Reason reason)
    {
        if (result.IsNotSuccessful)
        {
            return result.Reason;
        }

        if (condition(result.Value))
        {
            return reason;
        }

        return result.Value;
    }

    public static async Task<R<TRValue>> ReasonIfAsync<TRValue>(this R<TRValue> result, Func<TRValue, Task<bool>> condition, Reason reason)
    {
        if (result.IsNotSuccessful)
        {
            return result.Reason;
        }

        if (await condition(result.Value))
        {
            return reason;
        }

        return result.Value;
    }
}
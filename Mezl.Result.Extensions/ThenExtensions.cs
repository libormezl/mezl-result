namespace Mezl.Result.Extensions;

public static class ThenDoExtensions
{
    public static async Task<R<TRValue>> ThenDoAsync<TRValue>(this Task<R<TRValue>> resultTask, Func<TRValue, Task> onValue)
    {
        var result = await resultTask;
        if (result.IsNotSuccessful)
        {
            return result.Reason;
        }

        await onValue(result.Value)
            .ConfigureAwait(false);

        return result.Value;
    }

    public static async Task<R<TRValue>> ThenDo<TRValue>(this Task<R<TRValue>> resultTask, Action<TRValue> onValue)
    {
        var result = await resultTask;
        if (result.IsNotSuccessful)
        {
            return result.Reason;
        }

        onValue(result.Value);
        return result.Value;
    }

    public static R<TRValue> ThenDo<TRValue>(this R<TRValue> result, Action<TRValue> onValue)
    {
        if (result.IsNotSuccessful)
        {
            return result.Reason;
        }

        onValue(result.Value);
        return result.Value; ;
    }
}
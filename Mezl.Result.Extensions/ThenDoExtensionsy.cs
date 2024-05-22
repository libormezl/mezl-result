namespace Mezl.Result.Extensions;

public static class ThenExtensions
{
    public static async Task<R<TNextValue>> ThenAsync<TRValue, TNextValue>(this Task<R<TRValue>> resultTask, Func<TRValue, Task<R<TNextValue>>> onValue)
    {
        var result = await resultTask;
        if (result.IsNotSuccessful)
        {
            return result.Reason;
        }

        return await onValue(result.Value)
            .ConfigureAwait(false);
    }

    public static async Task<R<TNextValue>> ThenAsync<TRValue, TNextValue>(this R<TRValue> result, Func<TRValue, Task<R<TNextValue>>> onValue)
    {
        if (result.IsNotSuccessful)
        {
            return result.Reason;
        }

        return await onValue(result.Value)
            .ConfigureAwait(false);
    }

    public static async Task<R<TNextValue>> Then<TRValue, TNextValue>(this Task<R<TRValue>> resultTask, Func<TRValue, R<TNextValue>> onValue)
    {
        var result = await resultTask;
        if (result.IsNotSuccessful)
        {
            return result.Reason;
        }

        return onValue(result.Value);
    }

    public static R<TNextValue> Then<TRValue, TNextValue>(this R<TRValue> result, Func<TRValue, R<TNextValue>> onValue)
    {
        if (result.IsNotSuccessful)
        {
            return result.Reason;
        }

        return onValue(result.Value);
    }
}
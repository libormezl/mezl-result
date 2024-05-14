namespace Mezl.Result.Extensions
{
    public static class HandleExtensions
    {
        public static async Task<TNextValue> HandleAsync<TRValue, TNextValue>(this Task<R<TRValue>> resultTask, Func<TRValue, Task<R<TNextValue>>> onValue, Func<R<TRValue>, Task<R<TNextValue>>> onError)
        {
            var result = await resultTask;
            if (result.IsNotSuccessful)
            {
                return await onError(result)
                    .ConfigureAwait(false);
            }

            return await onValue(result.Value)
                .ConfigureAwait(false);
        }

        public static async Task<TNextValue> HandleAsync<TRValue, TNextValue>(this Task<R<TRValue>> resultTask, Func<TRValue, Task<R<TNextValue>>> onValue, Func<R<TRValue>, R<TNextValue>> onError)
        {
            var result = await resultTask;
            if (result.IsNotSuccessful)
            {
                return onError(result);
            }

            return await onValue(result.Value)
                .ConfigureAwait(false);
        }

        public static async Task<TNextValue> HandleAsync<TRValue, TNextValue>(this R<TRValue> result, Func<TRValue, Task<R<TNextValue>>> onValue, Func<R<TRValue>, Task<R<TNextValue>>> onError)
        {
            if (result.IsNotSuccessful)
            {
                return await onError(result)
                    .ConfigureAwait(false);
            }

            return await onValue(result.Value)
                .ConfigureAwait(false);
        }

        public static async Task<TNextValue> HandleAsync<TRValue, TNextValue>(this R<TRValue> result, Func<TRValue, Task<R<TNextValue>>> onValue, Func<R<TRValue>, R<TNextValue>> onError)
        {
            if (result.IsNotSuccessful)
            {
                return onError(result);
            }

            return await onValue(result.Value)
                .ConfigureAwait(false);
        }

        public static async Task<TNextValue> HandleAsync<TRValue, TNextValue>(this Task<R<TRValue>> resultTask, Func<TRValue, R<TNextValue>> onValue, Func<R<TRValue>, Task<R<TNextValue>>> onError)
        {
            var result = await resultTask;
            if (result.IsNotSuccessful)
            {
                return await onError(result)
                    .ConfigureAwait(false);
            }

            return onValue(result.Value);
        }

        public static async Task<TNextValue> HandleAsync<TRValue, TNextValue>(this Task<R<TRValue>> resultTask, Func<TRValue, R<TNextValue>> onValue, Func<R<TRValue>, R<TNextValue>> onError)
        {
            var result = await resultTask;
            if (result.IsNotSuccessful)
            {
                return onError(result);
            }

            return onValue(result.Value);
        }

        public static TNextValue Handle<TRValue, TNextValue>(this R<TRValue> result, Func<TRValue, R<TNextValue>> onValue, Func<R<TRValue>, R<TNextValue>> onError)
        {
            if (result.IsNotSuccessful)
            {
                return onError(result);
            }

            return onValue(result.Value);
        }
    }
}
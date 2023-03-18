namespace Mezl.Result.Extensions
{
    public static class ResultExtensions
    {
        #region Maps
        public static R<T> OnSuccess<T, TU>(this R<TU> result, Func<TU, T> func, string callInfo = null)
        {
            if (result.IsNotSuccessful)
            {
                return result.Reason;
            }

            return func(result.Value);
        }

        public static R<TU> Mapa<T, TU>(this R<T> result, Func<T, R<TU>> func)
        {
            if (result.IsNotSuccessful)
            {
                return result.Reason;
            }

            return func(result.Value);
        }

        //public static R Map<T>(this R<T> result, Func<T, Result> func)
        //{
        //    if (result.IsNotSuccessful)
        //    {
        //        return result.Reason;
        //    }

        //    return func(result.Value);
        //}

        public static async Task<R<T>> MapAsync<T, TU>(this R<TU> result, Func<TU, Task<Result<T>>> func)
        {
            if (result.IsNotSuccessful)
            {
                return result.Reason;
            }

            return await func(result.Value);
        }

        public static async Task<R> MapAsync<TU>(this R<TU> result, Func<TU, Task<Result>> func)
        {
            if (result.IsNotSuccessful)
            {
                return result.Reason;
            }

            return await func(result.Value);
        }

        public static async Task<R<T>> MapAsync<T, TU>(this Task<R<TU>> resultTask, Func<TU, Task<T>> func)
        {
            var result = await resultTask;

            if (result.IsNotSuccessful)
            {
                return result.Reason;
            }

            return await func(result.Value);
        }

        public static async Task<R> MapAsync<TU>(this Task<R<TU>> resultTask)
        {
            var result = await resultTask;

            if (result.IsNotSuccessful)
            {
                return result.Reason;
            }

            return result.Reason;
        }

        #endregion

        #region Taps

        public static R<T> Tap<T>(this R<T> result, Action<T> action)
        {
            if (result.IsNotSuccessful)
            {
                return result.Reason;
            }

            action(result.Value);

            return result;
        }

        public static async Task<R<T>> TapAsync<T>(this R<T> result, Func<T, Task> action)
        {
            if (result.IsNotSuccessful)
            {
                return result.Reason;
            }

            await action(result.Value);

            return result;
        }

        public static async Task<R<T>> TapAsync<T>(this Task<R<T>> resultTask, Func<T, Task> action)
        {
            var result = await resultTask;

            if (result.IsNotSuccessful)
            {
                return result.Reason;
            }

            await action(result.Value);

            return result;
        }

        public static async Task<R> TapAsync(this Task<R> resultTask, Action action)
        {
            var result = await resultTask;

            if (result.IsNotSuccessful)
            {
                return result.Reason;
            }

            action();

            return result;
        }

        public static async Task<R<T>> TapAsync<T>(this Task<R<T>> resultTask, Action<T> action)
        {
            var result = await resultTask;

            if (result.IsNotSuccessful)
            {
                return result.Reason;
            }

            action(result.Value);

            return result;
        }

        #endregion

        #region Tap Check

        public static R<T> TapCheck<T>(this R<T> result, Func<T, R> func)
        {
            if (result.IsNotSuccessful)
            {
                return result.Reason;
            }

            var check = func(result.Value);

            if (check.IsNotSuccessful)
            {
                return check.Reason;
            }

            return result;
        }

        public static async Task<R<T>> TapCheckAsync<T>(this R<T> result, Func<T, Task<R>> func)
        {
            if (result.IsNotSuccessful)
            {
                return result.Reason;
            }

            var check = await func(result.Value);
            if (check.IsNotSuccessful)
            {
                return check.Reason;
            }

            return result;
        }

        public static async Task<R<T>> TapCheckAsync<T>(this Task<R<T>> resultTask, Func<T, Task<R>> func)
        {
            var result = await resultTask;

            if (result.IsNotSuccessful)
            {
                return result.Reason;
            }

            var check = await func(result.Value);
            if (check.IsNotSuccessful)
            {
                return check.Reason;
            }

            return result;
        }

        public static async Task<R<T>> TapCheckAsync<T>(this Task<R<T>> resultTask, Func<T, R> func)
        {
            var result = await resultTask;

            if (result.IsNotSuccessful)
            {
                return result.Reason;
            }

            var check = func(result.Value);
            if (check.IsNotSuccessful)
            {
                return check.Reason;
            }

            return result;
        }

        #endregion
    }
}

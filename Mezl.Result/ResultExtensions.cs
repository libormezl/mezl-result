using System.Reflection;
using System.Runtime.CompilerServices;

namespace Mezl.Result
{
    public static class ResultExtensions
    {
        #region Maps

        public static Result<T> Map<T, TU>(this Result<TU> result, Func<TU, Result<T>> func)
        {
            if (result.IsNotSuccessful)
            {
                return result.Reason;
            }

            return func(result.Value);
        }

        public static Result Map<T>(this Result<T> result, Func<T, Result> func)
        {
            if (result.IsNotSuccessful)
            {
                return result.Reason;
            }

            return func(result.Value);
        }

        public static async Task<Result<T>> MapAsync<T, TU>(this Result<TU> result, Func<TU, Task<Result<T>>> func)
        {
            if (result.IsNotSuccessful)
            {
                return result.Reason;
            }

            return await func(result.Value);
        }

        public static async Task<Result> MapAsync<TU>(this Result<TU> result, Func<TU, Task<Result>> func)
        {
            if (result.IsNotSuccessful)
            {
                return result.Reason;
            }

            return await func(result.Value);
        }

        public static async Task<Result<T>> MapAsync<T, TU>(this Task<Result<TU>> resultTask, Func<TU, Task<T>> func)
        {
            var result = await resultTask;

            if (result.IsNotSuccessful)
            {
                return result.Reason;
            }

            return await func(result.Value);
        }

        public static async Task<Result> MapAsync<TU>(this Task<Result<TU>> resultTask)
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

        public static Result<T> Tap<T>(this Result<T> result, Action<T> action)
        {
            if (result.IsNotSuccessful)
            {
                return result.Reason;
            }

            action(result.Value);

            return result;
        }

        public static async Task<Result<T>> TapAsync<T>(this Result<T> result, Func<T, Task> action)
        {
            if (result.IsNotSuccessful)
            {
                return result.Reason;
            }

            await action(result.Value);

            return result;
        }

        public static async Task<Result<T>> TapAsync<T>(this Task<Result<T>> resultTask, Func<T, Task> action)
        {
            var result = await resultTask;

            if (result.IsNotSuccessful)
            {
                return result.Reason;
            }

            await action(result.Value);

            return result;
        }

        public static async Task<Result> TapAsync(this Task<Result> resultTask, Action action)
        {
            var result = await resultTask;

            if (result.IsNotSuccessful)
            {
                return result.Reason;
            }

            action();

            return result;
        }

        public static async Task<Result<T>> TapAsync<T>(this Task<Result<T>> resultTask, Action<T> action)
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

        public static Result<T> TapCheck<T>(this Result<T> result, Func<T, Result> func)
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

        public static async Task<Result<T>> TapCheckAsync<T>(this Result<T> result, Func<T, Task<Result>> func)
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

        public static async Task<Result<T>> TapCheckAsync<T>(this Task<Result<T>> resultTask, Func<T, Task<Result>> func)
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

        public static async Task<Result<T>> TapCheckAsync<T>(this Task<Result<T>> resultTask, Func<T, Result> func)
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


        public static Result<T> OnError<T>(this Result<T> result, Action<Reason> action)
        {
            if (result.IsNotSuccessful)
            {
                return result.Reason;
            }

            return result;
        }

        public static async Task<Result<T>> OnErrorAsync<T>(this Result<T> result, Func<Reason, Task> action)
        {
            if (result.IsNotSuccessful)
            {
                await action(result.Reason);
                return result.Reason;
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string CreateCallStack(string methodName, int lineNumber, string filePath)
        {
            return $"{filePath} -> {methodName} -> {lineNumber}";
        }
    }
}

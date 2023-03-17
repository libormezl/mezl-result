﻿namespace Mezl.Result.Extensions
{
    public static class BasicResultExtensions
    {
        public static bool Is<T>(this R result) where T : Reason
        {
            return result.IsNotSuccessful && result.Reason is T;
        }

        public static bool IsNot<T>(this R result) where T : Reason
        {
            return result.IsNotSuccessful && result.Reason is not T;
        }
    }
}
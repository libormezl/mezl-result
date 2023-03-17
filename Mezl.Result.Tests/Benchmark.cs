using System.Diagnostics;
using Mezl.Result.Handler;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Mezl.Result.Tests
{
    public class Benchmark
    {
        private class ReasonNotImplemented : Reason { }

        private readonly ITestOutputHelper _testOutputHelper;

        public Benchmark(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private string MethodThrowingException(int counter)
        {
            if (counter == 0)
            {
                throw new NotImplementedException();
            }

            return MethodThrowingException(--counter);
        }

        private R<string> MethodReturningResult(int counter)
        {
            if (counter == 0)
            {
                return Reason.New<ReasonNotImplemented>();
            }

            var result = MethodReturningResult(--counter);
            if (result.IsNotSuccessful)
            {
                return result.Reason;
            }

            throw new InvalidOperationException("This should not happen in this test");
        }

        private const int CallStackSize = 0;

        [Theory(Skip = "Integration")]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(10000)]
        [InlineData(1000000)]
        public void MeasureWithException(int count)
        {
            var stopwatch = Stopwatch.StartNew();

            for (var i = 0; i < count; i++)
            {
                try
                {
                    MethodThrowingException(CallStackSize);
                }
                catch
                {
                    // ignored
                }
            }

            stopwatch.Stop();

            _testOutputHelper.WriteLine($"Count: {count}, Elapsed {stopwatch.ElapsedMilliseconds} ms");
        }

        [Theory(Skip = "Integration")]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(10000)]
        [InlineData(1000000)]
        public void MeasureWithResult(int count)
        {
            var stopwatch = Stopwatch.StartNew();

            for (var i = 0; i < count; i++)
            {
                var result = MethodReturningResult(CallStackSize);
                if (result.IsNotSuccessful)
                {
                    //var aa = result.Reason.PrintCallStack();
                    // do something
                }
            }

            stopwatch.Stop();

            _testOutputHelper.WriteLine($"Count: {count}, Elapsed {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
using System.Reflection;
using BenchmarkDotNet.Attributes;

namespace ExpressionTrees.ConsoleApp.Reflection
{
    public class Benchmarks
    {
        [Benchmark(Description = "Reflection", Baseline = true)]
        public int Reflection() => (int)typeof(Command)
            .GetMethod("Execute", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(new Command(), null);

        [Benchmark(Description = "Reflection (cached)")]
        public int Cached() => ReflectionCached.CallExecute(new Command());

        [Benchmark(Description = "Reflection (delegate)")]
        public int Delegate() => ReflectionDelegate.CallExecute(new Command());

        [Benchmark(Description = "Expressions")]
        public int Expressions() => ExpressionTrees.CallExecute(new Command());
    }
}

using BenchmarkDotNet.Attributes;

namespace ExpressionTrees.ConsoleApp.Generics
{
    public class GenericBenchmarks
    {
        [Benchmark(Description = "Static", Baseline = true)]
        [Arguments(13.37)]
        public double Static(double x) => Generic.ThreeFourths(x);

        [Benchmark(Description = "Expressions")]
        [Arguments(13.37)]
        public double Expressions(double x) => Generic.Of(x);

        [Benchmark(Description = "Dynamic")]
        [Arguments(13.37)]
        public dynamic Dynamic(dynamic x) => Generic.ThreeFourths(x);
    }
}

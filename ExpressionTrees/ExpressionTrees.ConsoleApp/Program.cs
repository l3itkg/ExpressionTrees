using System;
using System.Linq.Expressions;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using ExpressionTrees.ConsoleApp.DSL;
using ExpressionTrees.ConsoleApp.Generics;
using ExpressionTrees.ConsoleApp.Reflection;

namespace ExpressionTrees.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Constructing Expression Trees

            //var greet = Greeting.ConstructGreetingFunction();
            //Console.WriteLine(greet("ExpressionTrees"));
            //Console.WriteLine(greet(""));

            #endregion

            #region Reflection

            //BenchmarkRunner.Run<Benchmarks>();

            #endregion

            #region Generic

            //Console.WriteLine($"ThreeFourths(18) - {ThreeFourths(18)}");
            //Console.WriteLine($"ThreeFourths(6.66) - {ThreeFourths(6.66)}");
            //Console.WriteLine($"ThreeFourths(100M) - {ThreeFourths(100M)}");

            //BenchmarkRunner.Run<GenericBenchmarks>();

            #endregion

            #region DSL

            //Console.WriteLine(SimpleCalculator.Run("2+2"));
            //Console.WriteLine(SimpleCalculator.Run("2*2+2"));
            //Console.WriteLine(SimpleCalculator.Run("3/4+5"));


            #endregion

            #region Visitor

            ////Expression<Func<double>> expr = () => Math.Sin(Guid.NewGuid().GetHashCode()) / 10;
            ////new Visitor.Visitor().Visit(expr);


            //Expression<Func<double>> expr = () => Math.Sin(Guid.NewGuid().GetHashCode()) / 10;
            //var result = expr.Compile()();

            //Console.WriteLine($"Old expression: {expr}");
            //Console.WriteLine($"Old result: {result}");

            //var newExpr = (Expression<Func<double>>)new Visitor.Visitor().Visit(expr);
            //var newResult = newExpr.Compile()();

            //Console.WriteLine("");
            //Console.WriteLine($"New expression: {newExpr}");
            //Console.WriteLine($"New result value: {newResult}");

            #endregion

            #region LogicOperators

            Expression<Func<int, bool>> e1 = x => x > 3;
            Expression<Func<int, bool>> e2 = x => x / 2 == 3;

            var combined = Expression.OrElse(e1.Body, e2.Body);
            var lambda = Expression.Lambda<Func<int, bool>>(combined);
            var method = lambda.Compile();

            #endregion
        }

        public static T ThreeFourths<T>(T x)
        {
            var param = Expression.Parameter(typeof(T));
            var three = Expression.Convert(Expression.Constant(3), typeof(T));
            var four = Expression.Convert(Expression.Constant(4), typeof(T));
            var operation = Expression.Divide(Expression.Multiply(param, three), four);
            var lambda = Expression.Lambda<Func<T, T>>(operation, param);
            var func = lambda.Compile();
            return func(x);
        }
    }

    //Visited binary expression: (Sin(Convert(NewGuid().GetHashCode(), Double)) / 10)
    //Visited method call: Sin(Convert(NewGuid().GetHashCode(), Double))
    //Visited method call: NewGuid().GetHashCode()
    //Visited method call: NewGuid()

    //Old expression: () => (Sin(Convert(NewGuid().GetHashCode(), Double)) / 10)
    //Old result: -0.05029024982775744

    //New expression: () => (Cos(Convert(NewGuid().GetHashCode(), Double)) / 10)
    //New result value: 0.09772502935634625


}
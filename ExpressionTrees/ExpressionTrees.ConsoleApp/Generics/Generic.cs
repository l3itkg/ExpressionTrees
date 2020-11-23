using System;
using System.Linq.Expressions;

namespace ExpressionTrees.ConsoleApp.Generics
{
    public static class Generic
    {
        private static class Impl<T>
        {
            public static Func<T, T> Of { get; }

            static Impl()
            {
                var param = Expression.Parameter(typeof(T));

                var three = Expression.Convert(Expression.Constant(3), typeof(T));
                var four = Expression.Convert(Expression.Constant(4), typeof(T));

                var operation = Expression.Divide(Expression.Multiply(param, three), four);

                var lambda = Expression.Lambda<Func<T, T>>(operation, param);

                Of = lambda.Compile();
            }
        }

        public static T Of<T>(T x) => Impl<T>.Of(x);


        public static int ThreeFourths(int x) => 3 * x / 4;

        public static long ThreeFourths(long x) => 3 * x / 4;

        public static float ThreeFourths(float x) => 3 * x / 4;

        public static double ThreeFourths(double x) => 3 * x / 4;

        public static decimal ThreeFourths(decimal x) => 3 * x / 4;

        public static dynamic ThreeFourths(dynamic x) => 3 * x / 4;
    }
}
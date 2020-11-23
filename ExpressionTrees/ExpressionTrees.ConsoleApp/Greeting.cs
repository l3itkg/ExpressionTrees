using System;
using System.Linq.Expressions;

namespace ExpressionTrees.ConsoleApp
{
    public static class Greeting
    {
        private static string Greet(string personName)
        {
            return !string.IsNullOrEmpty(personName)
                ? "Hi, " + personName
                : null;
        }

        public static Func<string, string> ConstructGreetingFunction()
        {
            var parameter = Expression.Parameter(typeof(string), "personName");

            var isNullOrEmpty = typeof(string).GetMethod(nameof(string.IsNullOrEmpty));

            var condition = Expression.Not(Expression.Call(isNullOrEmpty, parameter));

            var concat = typeof(string).GetMethod(nameof(string.Concat), new[] { typeof(string), typeof(string) });

            var trueClause = Expression.Call(concat, Expression.Constant("Hi, "), parameter);

            var falseClause = Expression.Constant(null, typeof(string));

            var conditional = Expression.Condition(condition, trueClause, falseClause);

            var lambda = Expression.Lambda<Func<string, string>>(conditional, parameter);

            return lambda.Compile();
        }
    }
}
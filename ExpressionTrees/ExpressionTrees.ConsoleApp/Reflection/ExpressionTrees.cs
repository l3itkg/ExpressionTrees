using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTrees.ConsoleApp.Reflection
{
    public static class ExpressionTrees
    {
        private static MethodInfo ExecuteMethod { get; } = typeof(Command)
            .GetMethod("Execute", BindingFlags.NonPublic | BindingFlags.Instance);

        private static Func<Command, int> Impl { get; }

        static ExpressionTrees()
        {
            var instance = Expression.Parameter(typeof(Command));
            var call = Expression.Call(instance, ExecuteMethod);
            Impl = Expression.Lambda<Func<Command, int>>(call, instance).Compile();
        }

        public static int CallExecute(Command command) => Impl(command);
    }
}
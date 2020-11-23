using System;
using System.Reflection;

namespace ExpressionTrees.ConsoleApp.Reflection
{
    public static class ReflectionDelegate
    {
        private static MethodInfo ExecuteMethod { get; } = typeof(Command)
            .GetMethod("Execute", BindingFlags.NonPublic | BindingFlags.Instance);

        private static Func<Command, int> Impl { get; } =
            (Func<Command, int>)Delegate.CreateDelegate(typeof(Func<Command, int>), ExecuteMethod);

        public static int CallExecute(Command command) => Impl(command);
    }
}

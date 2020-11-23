using System.Reflection;

namespace ExpressionTrees.ConsoleApp.Reflection
{
    public class ReflectionCached
    {
        private static MethodInfo ExecuteMethod { get; } = typeof(Command)
            .GetMethod("Execute", BindingFlags.NonPublic | BindingFlags.Instance);

        public static int CallExecute(Command command) => (int)ExecuteMethod.Invoke(command, null);
    }
}

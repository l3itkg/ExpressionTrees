using System.Reflection;

namespace ExpressionTrees.ConsoleApp.Reflection
{
    public static class Reflection
    {
        public static int CallExecute(Command command) =>
            (int)typeof(Command)
                .GetMethod("Execute", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(command, null);
    }
}
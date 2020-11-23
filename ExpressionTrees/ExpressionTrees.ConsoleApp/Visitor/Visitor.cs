using System;
using System.Linq.Expressions;

namespace ExpressionTrees.ConsoleApp.Visitor
{
    public class Visitor : ExpressionVisitor
    {
        
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var newMethodCall = node.Method == typeof(Math).GetMethod(nameof(Math.Sin))
                ? typeof(Math).GetMethod(nameof(Math.Cos))
                : node.Method;

            return Expression.Call(newMethodCall, node.Arguments);
        }
    }


    //protected override Expression VisitBinary(BinaryExpression node)
    //{
    //    Console.WriteLine($"Visited binary expression: {node}");

    //    return base.VisitBinary(node);
    //}

    //protected override Expression VisitMethodCall(MethodCallExpression node)
    //{
    //    Console.WriteLine($"Visited method call: {node}");

    //    return base.VisitMethodCall(node);
    //}
}

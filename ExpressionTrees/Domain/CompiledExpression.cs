using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Domain
{
    public static class CompiledExpressions<TIn, TOut>
    {
        private static readonly ConcurrentDictionary<Expression<Func<TIn, TOut>>, Func<TIn, TOut>> Cache =
            new ConcurrentDictionary<Expression<Func<TIn, TOut>>, Func<TIn, TOut>>();

        public static Func<TIn, TOut> AsFunc(Expression<Func<TIn, TOut>> expression) =>
            Cache.GetOrAdd(expression, expression.Compile());
    }

    public static class ExpressionExtensions
    {
        private static readonly ConcurrentDictionary<string, object> Cache
            = new ConcurrentDictionary<string, object>();

        public static Func<TIn, TOut> AsFunc<TIn, TOut>(this Expression<Func<TIn, TOut>> expr)
            => CompiledExpressions<TIn, TOut>.AsFunc(expr);

        public static bool Is<T>(this T entity, Expression<Func<T, bool>> expr)
            => AsFunc(expr).Invoke(entity);

        public static Expression<Func<TDestination, TReturn>> From<TSource, TDestination, TReturn>(
            this Expression<Func<TSource, TReturn>> source, Expression<Func<TDestination, TSource>> mapFrom)
            => Expression.Lambda<Func<TDestination, TReturn>>(
                Expression.Invoke(source, mapFrom.Body), mapFrom.Parameters);
    }
}

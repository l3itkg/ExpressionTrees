using System;
using System.Linq;
using System.Linq.Expressions;

namespace Domain
{
    public static class QueryExpressions
    {
        public static Expression<Func<TIn, TOut>> Compose<TIn, TInOut, TOut>(
            this Expression<Func<TIn, TInOut>> input,
            Expression<Func<TInOut, TOut>> inOutOut)
        {
            var param = Expression.Parameter(typeof(TIn), null);
            var invoke = Expression.Invoke(input, param);
            var res = Expression.Invoke(inOutOut, invoke);
            return Expression.Lambda<Func<TIn, TOut>>(res, param);
        }

        public static IQueryable<T> Where<T, TParam>(this IQueryable<T> queryable,
            Expression<Func<T, TParam>> prop, Expression<Func<TParam, bool>> where)
        {
            return queryable.Where(prop.Compose(where));
        }
    }
}

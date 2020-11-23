﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Domain
{
    public enum ComposeKind
    {
        And,
        Or
    }

    public static class Filter
    {
        public static IQueryable<TSubject> AutoFilter<TSubject, TPredicate>(
            this IQueryable<TSubject> query, TPredicate predicate, ComposeKind composeKind = ComposeKind.And)
        {
            var filtered = Conventions<TSubject>.Filter(query, predicate, composeKind);
            var orderBy = FastTypeInfo<TPredicate>.PublicProperties.FirstOrDefault(x => x.Name == "OrderBy");
            var propertyName = orderBy?.GetValue(predicate, null) as string;

            return propertyName == null
                ? filtered
                : Conventions<TSubject>.Sort(filtered, propertyName);
        }

        public static class Conventions<TSubject>
        {
            private static readonly MethodInfo Contains = typeof(string)
                .GetMethod("Contains", new[] { typeof(string) });

            public static IQueryable<TSubject> Filter<TPredicate>(IQueryable<TSubject> query, TPredicate predicate,
                ComposeKind composeKind = ComposeKind.And)
            {
                var filterProperties = FastTypeInfo<TPredicate>
                    .PublicProperties
                    .ToArray();

                var filterPropertiesNames = filterProperties
                    .Select(x => x.Name)
                    .ToArray();

                var modelType = typeof(TSubject);

                var parameter = Expression.Parameter(modelType);

                var props = FastTypeInfo<TSubject>
                    .PublicProperties
                    .Where(x => filterPropertiesNames.Contains(x.Name))
                    .Select(x => new
                    {
                        Property = x,
                        Value = filterProperties.Single(y => y.Name == x.Name).GetValue(predicate)
                    })
                    .Where(x => x.Value != null)
                    .Select(x =>
                    {
                        Expression property = Expression.Property(parameter, x.Property);
                        Expression value = Expression.Constant(x.Value);

                        value = Expression.Convert(value, property.Type);
                        var body = property.Type == typeof(string)
                            ? (Expression)Expression.Call(property, Contains, value)
                            : Expression.Equal(property, value);

                        return Expression.Lambda<Func<TSubject, bool>>(body, parameter);
                    })
                    .ToArray();

                if (!props.Any())
                {
                    return query;
                }

                var expr = composeKind == ComposeKind.And
                    ? props.Aggregate((c, n) => c.And(n))
                    : props.Aggregate((c, n) => c.Or(n));

                return query.Where(expr);
            }

            public static IOrderedQueryable<TSubject> Sort(IQueryable<TSubject> query, string propertyName)
            {
                (string, bool) GetSorting()
                {
                    var arr = propertyName.Split('.');
                    if (arr.Length == 1)
                        return (arr[0], false);
                    var sort = arr[1];
                    if (string.Equals(sort, "asc", StringComparison.CurrentCultureIgnoreCase))
                        return (arr[0], false);
                    if (string.Equals(sort, "desc", StringComparison.CurrentCultureIgnoreCase))
                        return (arr[0], true);
                    return (arr[0], false);
                }

                var (name, isDesc) = GetSorting();
                propertyName = name;

                var property = FastTypeInfo<TSubject>
                    .PublicProperties
                    .FirstOrDefault(x =>
                        string.Equals(x.Name, propertyName, StringComparison.CurrentCultureIgnoreCase));

                if (property == null)
                    throw new InvalidOperationException($"There is no public property \"{propertyName}\" " +
                                                        $"in type \"{typeof(TSubject)}\"");

                var parameter = Expression.Parameter(typeof(TSubject));
                var body = Expression.Property(parameter, propertyName);

                var lambda = FastTypeInfo<Expression>
                    .PublicMethods
                    .First(x => x.Name == "Lambda");

                lambda = lambda.MakeGenericMethod(typeof(Func<,>)
                    .MakeGenericType(typeof(TSubject), property.PropertyType));

                var expression = lambda.Invoke(null, new object[] { body, new[] { parameter } });

                var methodName = isDesc ? "OrderByDescending" : "OrderBy";

                var orderBy = typeof(Queryable)
                    .GetMethods()
                    .First(x => x.Name == methodName && x.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(TSubject), property.PropertyType);

                return (IOrderedQueryable<TSubject>)orderBy.Invoke(query, new object[] { query, expression });
            }
        }
    }
}

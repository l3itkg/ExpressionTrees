using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Domain
{
    public delegate T ObjectActivator<out T>(params object[] args);

    public static class FastTypeInfo<T>
    {
        private static readonly Attribute[] _attributes;

        private static readonly PropertyInfo[] _properties;

        private static readonly MethodInfo[] _methods;

        private static ConstructorInfo[] _constructors;

        private static ConcurrentDictionary<string, ObjectActivator<T>> _activators;

        static FastTypeInfo()
        {
            var type = typeof(T);
            _attributes = type.GetCustomAttributes().ToArray();

            _properties = type
                .GetProperties()
                .Where(x => x.CanRead && x.CanWrite)
                .ToArray();

            _methods = type.GetMethods()
                .Where(x => x.IsPublic && !x.IsAbstract)
                .ToArray();

            _constructors = typeof(T).GetConstructors();
            _activators = new ConcurrentDictionary<string, ObjectActivator<T>>();
        }

        public static PropertyInfo[] PublicProperties => _properties;

        public static MethodInfo[] PublicMethods => _methods;

        public static Attribute[] Attributes => _attributes;

        public static bool HasAttribute<TAttr>()
            where TAttr : Attribute
            => Attributes.Any(x => x.GetType() == typeof(TAttr));

        public static TAttr GetCustomAttribute<TAttr>()
            where TAttr : Attribute
            => (TAttr)_attributes.FirstOrDefault(x => x.GetType() == typeof(TAttr));
        #region Create


        private static ObjectActivator<T> GetActivator(ConstructorInfo ctor)
        {
            var type = ctor.DeclaringType;
            var paramsInfo = ctor.GetParameters();

            var param = Expression.Parameter(typeof(object[]), "args");

            var argsExp = new Expression[paramsInfo.Length];

            for (var i = 0; i < paramsInfo.Length; i++)
            {
                var index = Expression.Constant(i);
                var paramType = paramsInfo[i].ParameterType;

                Expression paramAccessorExp = Expression.ArrayIndex(param, index);
                Expression paramCastExp = Expression.Convert(paramAccessorExp, paramType);

                argsExp[i] = paramCastExp;
            }

            var newExp = Expression.New(ctor, argsExp);

            var lambda = Expression.Lambda(typeof(ObjectActivator<T>), newExp, param);

            var compiled = (ObjectActivator<T>)lambda.Compile();
            return compiled;
        }

        public static Delegate CreateMethod(MethodInfo method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            if (!method.IsStatic)
            {
                throw new ArgumentException("The provided method must be static.", nameof(method));
            }

            if (method.IsGenericMethod)
            {
                throw new ArgumentException("The provided method must not be generic.", nameof(method));
            }

            var parameters = method.GetParameters()
                .Select(p => Expression.Parameter(p.ParameterType, p.Name))
                .ToArray();

            var call = Expression.Call(null, method, parameters);
            return Expression.Lambda(call, parameters).Compile();
        }

        #endregion

        public static Func<TObject, TProperty> PropertyGetter<TObject, TProperty>(string propertyName)
        {
            var paramExpression = Expression.Parameter(typeof(TObject), "value");

            var propertyGetterExpression = Expression.Property(paramExpression, propertyName);

            var result = Expression.Lambda<Func<TObject, TProperty>>(propertyGetterExpression, paramExpression)
                .Compile();

            return result;
        }

        public static Action<TObject, TProperty> PropertySetter<TObject, TProperty>(string propertyName)
        {
            var paramExpression = Expression.Parameter(typeof(TObject));
            var paramExpression2 = Expression.Parameter(typeof(TProperty), propertyName);
            var propertyGetterExpression = Expression.Property(paramExpression, propertyName);
            var result = Expression.Lambda<Action<TObject, TProperty>>
            (
                Expression.Assign(propertyGetterExpression, paramExpression2), paramExpression, paramExpression2
            ).Compile();

            return result;
        }
    }
}

namespace SystemDot.EventSourcing.Bootstrapping
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Reflection;

    public static class IEnumerableExtensions
    {
        public static IEnumerable ThatHaveAttribute<T>(this IEnumerable enumerable) where T : Attribute
        {
            return enumerable
                .Cast<object>()
                .Where(item => item.GetType().GetTypeInfo().GetCustomAttributes(typeof(T)).Any());
        }
    }
}
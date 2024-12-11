using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

// adapted from: https://stackoverflow.com/a/53299290
public static class Memoize
{
    internal static ConditionalWeakTable<object, ConcurrentDictionary<string, object>> _weakCache =
        [];

    public static TResult Memoized<T1, TResult>(
        this object context,
        T1 arg,
        Func<T1, TResult> f,
        [CallerMemberName] string? cacheKey = null)
        where T1 : notnull
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(cacheKey);

        var objCache = _weakCache.GetOrCreateValue(context);

        var methodCache = (ConcurrentDictionary<T1, TResult>)objCache
            .GetOrAdd(cacheKey, _ => new ConcurrentDictionary<T1, TResult>());

        return methodCache.GetOrAdd(arg, f);
    }
}
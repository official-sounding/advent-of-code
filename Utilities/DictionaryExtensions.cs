public static class DictionaryExtensions
{

    public static V GetOrAdd<K, V>(this Dictionary<K, V> dictionary, K key, Func<K, V> valueBuilder) where K : notnull
    {
        if (!dictionary.TryGetValue(key, out var value))
        {
            value = valueBuilder(key);
            dictionary[key] = value;
        }

        return value;
    }
}
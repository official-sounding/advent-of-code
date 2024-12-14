public class NumberUtils
{
    public static int Mod(int value, int mod)
    {
        var rem = value % mod;
        return rem < 0 ? rem + mod : rem;
    }

    public static long Mod(long value, long mod)
    {
        var rem = value % mod;
        return rem < 0 ? rem + mod : rem;
    }
}
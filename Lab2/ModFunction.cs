namespace Mod
{
    public class ModFunction
    {
        public static long GetMod(long _base, long exp, long mod)
        {
            long x = 1;
            long y = _base;

            while (exp > 0)
            {
                if 
                    ((exp & 1) == 1)
                    x = (x * y) % mod;

                y = (y * y) % mod;
                exp = exp / 2;

            }
            return x % mod;
        }
    }
}
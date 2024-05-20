using LCD;
using Mod;

namespace Soloway
{
    public class SolowayTest
    {
        public static bool Test(long n, int k)
        {
            if (n == 2)
            {
                return true;
            }
            if (n < 2)
            {
                return false;
            }
            if (n != 2 && n % 2 == 0) {
                return false;
            }

            Random rand = new Random();

            for (int i = 0; i < k; i++)
            {
                long a = rand.NextInt64(2, n);
                long jacobi = (n + Jacobi.Jacobi.GetJacobi(a, n)) % n;
                long mod = ModFunction.GetMod(a, (n - 1) / 2, n);
                if (jacobi == 0 || mod != jacobi)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
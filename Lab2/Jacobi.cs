namespace Jacobi
{
    public class Jacobi
    {
        public static long GetJacobi(long a, long b)
        {
            if (b <= 0 || b % 2 == 0)
            {
                return 0;
            }

            long result = 1;

            if (a < 0)
            {
                a *= -1;
                if (b % 4 == 3)
                {
                    result *= -1;
                }
            }
            if (a == 1)
            {
                return result;
            }

            while (a != 0)
            {
                if (a < 0)
                {
                    a *= -1;
                    if (b % 4 == 3)
                    {
                        result *= -1;
                    }
                }

                while (a % 2 == 0)
                {
                    a /= 2;
                    if (b % 8 == 3 || b % 8 == 5)
                    {
                        result *= -1;
                    }
                }

                long temp = a;
                a = b;
                b = temp;

                if (a % 4 == 3 && b % 4 == 3)
                {
                    result *= -1;
                }

                a %= b;

                if (a > b / 2)
                {
                    a -= b;
                }
            }
            if (b == 1)
            {
                return result;
            }
            return 0;
        }
    }
}
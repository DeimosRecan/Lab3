namespace LCD
{
    public class LCD
    {
        public static long GetLCD(long a, long b)
        {
            long temp = 0;
            if (a > b)
            {
                temp = a;
                a = b;
                b = temp;
            }

            long resLcd = 1;

            for (long i = a; i >= 2; i--)
            {
                if (a % i == 0 && b % i == 0)
                {
                    resLcd = i;
                    break;
                }
            }
            return resLcd;
        }
    }
}
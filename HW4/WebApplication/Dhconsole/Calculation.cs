using System;

namespace HomeWork2
{
    public  class Calculation
    {
        public  bool IsInRange(ulong n, ulong p)
        {
            if (n > 0 && n < p) 
                return true;
            Console.WriteLine($"Out of range, try again.");
            return false;
        }

        public static int CountBits(ulong r)
        {
            int k = 0;
            while (r > 0)
            {
                r >>= 1;
                k++;
            }

            return k;
        }

        public ulong PowWithMod(ulong a, ulong b, ulong c)
        {
            ulong x = a % c;
            int k = CountBits(b) - 2;
            /* Left-to-Right binary method */
            while (k >= 0)
            {
                if (2 * Math.Log(x) > Math.Log(ulong.MaxValue))
                    throw new System.ArgumentException("Overflow Error.");
                x = (x * x) % c;
                if ((b >> k & 1) == 1)
                {
                    if (Math.Log(x) + Math.Log(a) > Math.Log(ulong.MaxValue))
                        throw new System.ArgumentException("Overflow Error.");
                    x = (x * a) % c;
                }

                k--;
            }

            return x;

        }



    }
}
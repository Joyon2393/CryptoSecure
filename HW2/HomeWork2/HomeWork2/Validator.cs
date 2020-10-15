using System;

namespace HomeWork2
{
    public  class Validator
    {

        public static Boolean IsInt(string a)
        {
            int result;
            bool parsedSuccessfully = int.TryParse(a, out result);
            if (parsedSuccessfully == false)
            {
                Console.WriteLine("Please Provide an integer");
                return false;
            }

            return true;
        }

        public  Boolean IsPrime(ulong n)
        {
            string v = n.ToString();
            int m, flag = 0;
            m = (int) (n / 2);
            if (IsInt(v))
            {
                for (int i = 2; i <= m; i++)
                {
                    if (n % (ulong) i == 0)
                    {
                        Console.WriteLine("It is not a prime");
                        flag = 1;
                        break;
                    }

                }

                if (flag == 0)
                {
                    return true;
                }

                return false;

            }

            return false;

        }

        public  bool IsPrimitiveRoot(ulong r, ulong p)
        {
            for (ulong i = 1; i < p; i++)
            {
                if (i * Math.Log(r) > Math.Log(ulong.MaxValue))
                    throw new System.ArgumentException("Overflow Error.");
                if ((ulong) Math.Pow(r, i) % p == 1)
                {
                    if (i == p - 1)
                        return true;
                    Console.WriteLine("Not a primitive root, try again.");
                    return false;
                }
            }

            Console.WriteLine("Not a primitive root, try again.");
            return false;



        }
        
    }
}
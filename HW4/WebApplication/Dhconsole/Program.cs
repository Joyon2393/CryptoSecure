using System;
using System.Xml.Schema;

namespace HomeWork2
{
    class Program
    {
        
        
        public static void Main(string[] args)
        {
            Console.WriteLine(" Hello Bob and Alice!!!");
            Console.WriteLine("Please enter common modulus p of a prime number");
            var q = Console.ReadLine();
            var p = Convert.ToUInt64(q);
            Validator validator = new Validator();
            Calculation calculation=new Calculation();
            while (!validator.IsPrime(p))
            {
                Console.WriteLine("Please give me a valid number");
                break;
            }
            Console.WriteLine($"your modulus number is {p}");
            Console.WriteLine($"Now, give me base g of a primitive root modulo {p}:");
            var s = Console.ReadLine();
            var r = Convert.ToUInt64(s);
           

            while(!validator.IsPrimitiveRoot(r,p))
            {
                Console.WriteLine("please give me a valid number");
                break;
            }
            
            Console.WriteLine(" Hello Alice!!!");
            Console.WriteLine($"Give me a secret integer between(0,{p})");
            var g = Console.ReadLine();
            var G = Convert.ToUInt64(s);
            while (!calculation.IsInRange(G, p))
            {
                Console.WriteLine("Out of Range");
                Console.WriteLine("please provide a valid number");
                break;
            }
            Console.WriteLine($"Alices secret number is {G}");
            ulong compA = calculation.PowWithMod(r, G, p);
            Console.WriteLine($"Alice's computed number is {compA}");
            
            Console.WriteLine(" Hello Bob!!!");
            Console.WriteLine($"Give me a secret integer between(0,{p})");
            var S = Console.ReadLine();
            var R = Convert.ToUInt64(s);
            while (!calculation.IsInRange(R, p))
            {
                Console.WriteLine("Out of Range");
                Console.WriteLine("please provide a valid number");
                break;
            }
            Console.WriteLine($"Bobs secret number is {R}");
            ulong compB = calculation.PowWithMod(r, R, p);
            Console.WriteLine($"Bob's computed number is {compB}");
            
            Console.WriteLine($"Alice , bob sent you {compB}");
            ulong secretAlice = calculation.PowWithMod(compB, G, p);
            
            Console.WriteLine($"Bob , Alice sent you {compA}");
            ulong secretBob = calculation.PowWithMod(compA, G, p);
            
            Console.WriteLine($"Alice has {secretAlice} and Bob has {secretBob}");
            



          






        }
    }
}
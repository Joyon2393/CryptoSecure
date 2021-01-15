using System;
using System.Text;

namespace homework3
{
     class Program
    {
        public static void Main(string[] args)
        {
           
            rsa();
            
            
            
           static void rsa()
          {

            ulong p = 0;
            ulong q = 0;
            Console.Write("RSA p value: ");
            var pValue = Console.ReadLine();
            Console.Write("RSA q value: ");
            var qValue = Console.ReadLine();

            if (!ulong.TryParse(pValue, out p) || !ulong.TryParse(qValue, out q))
            {
                Console.WriteLine("Invalid input.");
                rsa();
            }

            if (!CheckingPrime(p) || !CheckingPrime(q))
            {
                Console.WriteLine("P and q values must be prime.");
                rsa();
            }

            var n = p * q;
            var m = (p - 1) * (q - 1);

            ulong e;
            for (e = 2; e < ulong.MaxValue; e++)
            {
                if (GCD(m, e) == 1) break;
            }

            ulong d = 2;
            while (d < m)
            {
                if ((d*e) % m == 1)
                {
                    break;
                }
                d++;
            }

            Console.WriteLine($"Public key ({e},{n})");
            Console.WriteLine($"Private key ({d},{n})");

            var option = "";

            do
            {
                Console.Write("RSA keypair loaded. Select option \n 1) Encrypt and Decrypt \n 2) Bruteforce \n X) Exit \n > ");
                option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        Console.Write("Plain text: ");
                        var plainTextString = Console.ReadLine();
                        if (plainTextString == "") break;
                        byte[] combinedString = rsa_encrypt(plainTextString, e, n);
                        var base64String = Convert.ToBase64String(combinedString);
                        Console.WriteLine($"Encrypted: {Convert.ToBase64String(combinedString)}");
                        byte[] decryptedText = rsa_decrypt(base64String, d, n);
                        Console.WriteLine($"Decrypted: {Encoding.UTF8.GetString(decryptedText)}");
                        break;
                    case "2":
                        BruteForce();
                        break;
                        
                    default:
                        Console.WriteLine($"An unexpected value was entered ({option}). Please try again!");
                        break;
                }
            }while (option != "x" || option == "") ;


           }

           static byte[] rsa_encrypt(string text, ulong e, ulong n)
           {
               byte[] inputBytes = Encoding.ASCII.GetBytes(text);
               var result = new byte[text.Length];
               int i = 0;
               foreach (byte b in inputBytes)
               {
                   ulong encryptedChar = ModExponentiation(b,e, n);
                   result[i] = (byte) encryptedChar;
                   i++;
               }


               return result;
           }

           static byte[] rsa_decrypt(string base64String, ulong d, ulong n)
           {
               var inputBytes = Convert.FromBase64String(base64String);
               var result = new byte[base64String.Length];
               int i = 0;
               foreach (byte b in inputBytes)
               {
                   ulong decryptedChar = ModExponentiation(b,d, n);
                   result[i] = (byte) decryptedChar;
                   i++;
               }

 
               return result;
           }

           static ulong ModExponentiation(ulong x, ulong y, ulong p)
           {
               ulong res = 1;

               x = x % p;

               if (x == 0)
                   return 0;

               while (y > 0)
               {
                   if ((y & 1) == 1)
                       res = (res * x) % p;

                   y = y >> 1;
                   x = (x * x) % p;
               }
               return res;
           }

           static bool CheckingPrime(ulong n)
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
            static Boolean IsInt(string a)
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
            static ulong GCD(ulong a, ulong b)
            {
                ulong Remainder;
    
                while( b != 0 )
                {
                    Remainder = a % b;
                    a = b;
                    b = Remainder;
                }
      
                return a;
            }
             static bool IsBase64( string base64String) {
                
                if (string.IsNullOrEmpty(base64String) || base64String.Length % 4 != 0
                                                       || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
                    return false;
                try{
                    Convert.FromBase64String(base64String);
                    return true;
                }
                catch(Exception exception){
                    Console.WriteLine("Error");
                }
                return false;
            }

             static void BruteForce()
             {
                 
             }
             
        }
    }
}
using System;
using System.Text;

namespace Crypto
{
    public  class Rsa
    {
        
            public  byte[] rsa_encrypt(string text, ulong e, ulong n)
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(text);
                var result = new byte[text.Length];
                int i = 0;
                foreach (byte b in inputBytes)
                {
                    ulong encryptedChar = ModExponentiation(b, e, n);
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
                    ulong decryptedChar = ModExponentiation(b, d, n);
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




        }
    }

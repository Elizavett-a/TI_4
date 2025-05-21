using System;
using System.Text;
using System.Numerics;

namespace TI_Lab4
{
    public class EDS
    {
        private static BigInteger H0 = 100;

        public static BigInteger ComputeHash(byte[] textBytes, BigInteger modulus)
        {
            BigInteger currentHash = EDS.H0;

            foreach (byte b in textBytes)
            {
                currentHash = DSAlg.ModPow(currentHash + b, 2, modulus);
            }

            return currentHash;
        }

        public static BigInteger ComputeHash(string text, BigInteger modulus)
        {
            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            return ComputeHash(textBytes, modulus);
        }
    }
}
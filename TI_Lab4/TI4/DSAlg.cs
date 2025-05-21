using System.Text;
using System.Collections;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Numerics;

namespace TI_Lab4
{
    public class DSAlg
    {
        public static string CheckForNum(string str)
        {
            var result = new StringBuilder();
            foreach (char symbol in str)
            {
                if (char.IsDigit(symbol))
                {
                    result.Append(symbol);
                }
            }
            return result.ToString();
        }

        public static bool CheckD(BigInteger a, BigInteger b)
        {
            if ((a == 1) || (a > b)) return false;
            while (b != 0)
            {
                BigInteger temp = b;
                b = a % b;
                a = temp;
            }
            return a == 1;
        }

        // Проверка на простое число
        private static readonly Random random = new Random();

        public static bool IsPrime(BigInteger n, int k = 5)
        {
            if (n <= 1) return false;
            if (n == 2 || n == 3) return true;
            if (n % 2 == 0) return false;

            for (BigInteger i = 0; i < k; i++)
            {
                BigInteger a = random.Next(2, (int)(n - 1));

                if (ModPow(a, n - 1, n) != 1)
                    return false;
            }

            return true;
        }

        // Для поиска e
        public static BigInteger EuclidRev(BigInteger a, BigInteger b)
        {
            BigInteger d0 = a;
            BigInteger d1 = b;
            BigInteger x0 = 1;
            BigInteger x1 = 0;
            BigInteger y0 = 0;
            BigInteger y1 = 1;

            while (d1 > 1)
            {
                BigInteger q = d0 / d1;
                BigInteger d2 = d0 % d1;
                BigInteger x2 = x0 - q * x1;
                BigInteger y2 = y0 - q * y1;

                d0 = d1;
                d1 = d2;
                x0 = x1;
                x1 = x2;
                y0 = y1;
                y1 = y2;
            }

            return y1;
        }

        //Вычисление значения функции Эйлера
        public static BigInteger EulerAlg(BigInteger n)
        {
            BigInteger result = n; 

            for (BigInteger p = 2; p * p <= n; ++p)
            {
                if (n % p == 0)
                {
                    while (n % p == 0)
                    {
                        n /= p;
                    }
                    result -= result / p;
                }
            }

            if (n > 1)
            {
                result -= result / n;
            }

            return result;
        }

        //Возведение в степень по модулю
        public static BigInteger ModPow(BigInteger a, BigInteger exponent, BigInteger modulus)
        {
            if (modulus == 1) return 0;

            BigInteger result = 1;
            a = a % modulus;

            while (exponent > 0)
            {
                if (exponent % 2 == 1)
                    result = (result * a) % modulus;

                exponent >>= 1;
                a = (a * a) % modulus;
            }
            return result;
        }
    }
}
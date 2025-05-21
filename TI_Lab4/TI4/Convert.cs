using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TI_Lab4
{
    public static class Convert
    {
        public static string StringToDecimalBytes(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            byte[] bytes = Encoding.UTF8.GetBytes(input);
            return string.Join(" ", bytes.Select(b => (int)b));
        }

        public static string DecimalBytesToString(string decimalBytesString)
        {
            if (decimalBytesString == null)
                throw new ArgumentNullException(nameof(decimalBytesString));

            string[] parts = decimalBytesString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var bytes = new byte[parts.Length];
            for (int i = 0; i < parts.Length; i++)
            {
                if (!int.TryParse(parts[i], out int num) || num < 0 || num > 255)
                    throw new ArgumentException($"Некорректное число: {parts[i]} (допустимо 0-255)");

                bytes[i] = (byte)num;
            }

            return Encoding.UTF8.GetString(bytes);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex06.ProductManagement.Helpers
{
    public class InputHelper
    {
        public static string ReadNonEmptyString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(input))
                    return input;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Giá trị không được để trống! Vui lòng nhập lại.");
                Console.ResetColor();
            }
        }

        public static decimal ReadDecimal(string prompt, decimal min = 0)
        {
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine()?.Trim(), out decimal value) && value >= min)
                    return value;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Vui lòng nhập số thực hợp lệ (>= {min:N0}).");
                Console.ResetColor();
            }
        }

        public static int ReadInt(string prompt, int min = 0)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine()?.Trim(), out int value) && value >= min)
                    return value;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Vui lòng nhập số nguyên hợp lệ (>= {min}).");
                Console.ResetColor();
            }
        }

        public static HashSet<string> ReadTags(string prompt)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(input))
                return new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            return input
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim().ToLower())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
        }
    }
}

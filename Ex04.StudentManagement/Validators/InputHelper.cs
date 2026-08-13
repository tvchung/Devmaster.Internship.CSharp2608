using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace Ex04.StudentManagement.Helpers
{
    public class InputHelper
    {
        public static string ReadNonEmptyString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(input)) return input;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Dữ liệu không được để trống. Vui lòng nhập lại!");
                Console.ResetColor();
            }
        }

        // Phone
        public static string ReadPhoneNumber(string prompt)
        {
            while (true)
            {
                string phone = ReadNonEmptyString(prompt);
                // Kiểm tra số điện thoại từ 9 đến 11 chữ số
                if (Regex.IsMatch(phone, @"^\d{9,11}$")) return phone;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Số điện thoại không hợp lệ! (Phải chứa từ 9 đến 11 chữ số).");
                Console.ResetColor();
            }
        }
        // email
        public static string ReadEmail (string prompt)
        {
            while (true)
            {
                string email = ReadNonEmptyString(prompt);
                // Kiểm tra ^[^@\s]+@[^@\s]+\.[^@\s]+$
                if (Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]{2,4}$")) return email;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Email không đúng định  dạng(...).");
                Console.ResetColor();
            }
        }
        public static int ReadInt(string prompt, int min = int.MinValue, int max = int.MaxValue)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int result) && result >= min && result <= max)
                {
                    return result;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Giá trị không hợp lệ! Vui lòng nhập số nguyên từ {min} đến {max}.");
                Console.ResetColor();
            }
        }

        public static decimal ReadDecimal(string prompt, decimal min = 0, decimal max = decimal.MaxValue)
        {
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out decimal result) && result >= min && result <= max)
                {
                    return result;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Giá trị không hợp lệ! Vui lòng nhập số trong khoảng [{min} - {max}].");
                Console.ResetColor();
            }
        }
        public static double ReadDouble(string prompt, double min = 0, double max = double.MaxValue)

        {
            while (true)
            {
                Console.Write(prompt);
                if (double.TryParse(Console.ReadLine(), out double result) && result >= min && result <= max)
                {
                    return result;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Giá trị không hợp lệ! Vui lòng nhập số trong khoảng [{min} - {max}].");
                Console.ResetColor();
            }
        }
    }
}

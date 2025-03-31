using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AdTorrBot.BotTelegram
{
  public  class InputTextValidator
    {
        public static bool IsValidIPv6(string ipString)
        {
            if (string.IsNullOrWhiteSpace(ipString))
                return false;

            // Попытка преобразовать строку в IPAddress
            if (IPAddress.TryParse(ipString, out IPAddress address))
            {
                // Проверяем, является ли адрес IPv6
                return address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6;
            }

            return false;
        }
        public static bool IsValidIPv4(string ipString)
        {
            if (string.IsNullOrWhiteSpace(ipString))
                return false;

            // Попытка преобразовать строку в IPAddress
            if (IPAddress.TryParse(ipString, out IPAddress address))
            {
                // Проверяем, является ли адрес IPv4
                return address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork;
            }

            return false;
        }
        public static bool IsValidPath(string path)
        {
            // Максимальная длина пути
            if (string.IsNullOrWhiteSpace(path))
            {
                Console.WriteLine("Путь пустой или состоит только из пробелов.");
                return false;
            }

            if (path.Length > 4096)
            {
                Console.WriteLine("Путь превышает максимальную длину в 4096 символов.");
                return false;
            }

            // Проверка запрещённых символов
            char[] invalidChars = { '\0' }; // Только NULL-символ недопустим
            if (path.IndexOfAny(invalidChars) >= 0)
            {
                Console.WriteLine("Путь содержит запрещённые символы.");
                return false;
            }

            // Проверка имени каждой части пути
            string[] parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                if (string.IsNullOrWhiteSpace(part))
                {
                    Console.WriteLine("Один из сегментов пути пустой.");
                    return false;
                }

                if (part.Length > 255)
                {
                    Console.WriteLine($"Один из сегментов пути '{part}' превышает допустимую длину в 255 символов.");
                    return false;
                }

                if (part == "." || part == "..")
                {
                    Console.WriteLine($"Сегмент пути '{part}' является недопустимым ('.' или '..').");
                    return false;
                }

                if (part.StartsWith("-"))
                {
                    Console.WriteLine($"Сегмент пути '{part}' начинается с недопустимого символа '-'.");
                    return false;
                }
            }

            Console.WriteLine("Путь валиден.");
            return true;
        }




        public static bool ValidateLoginAndPassword(string login)
        {
            // Проверка длины строки
            if (login.Length > 20)
            {
                Console.WriteLine("Login/password не может быть длиннее 20 символов.");
                return false;  
                    ;
            }

            // Проверка регулярного выражения
            var regex = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9]+$");
            if (!regex.IsMatch(login))
            {
                Console.WriteLine("Login/password может содержать только английские буквы и цифры.");
                return false;
               
                
            }

           
            return true;
        }
    }
}

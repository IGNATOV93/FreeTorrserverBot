using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdTorrBot.BotTelegram
{
  public  class InputTextValidator
    {
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
            if (login.Length > 10)
            {
                Console.WriteLine("Login/password не может быть длиннее 10 символов.");
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

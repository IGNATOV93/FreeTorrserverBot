using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdTorrBot.BotTelegram
{
  public  class InputTextValidator
    {
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdTorrBot.BotTelegram
{
  public  class InputTextValidator
    {
        public static bool ValidateLogin(string login)
        {
            // Проверка длины строки
            if (login.Length > 20)
            {
                Console.WriteLine("Login не может быть длиннее 20 символов.");
                return false;  
                    ;
            }

            // Проверка регулярного выражения
            var regex = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9]+$");
            if (!regex.IsMatch(login))
            {
                Console.WriteLine("Login может содержать только английские буквы и цифры.");
                return false;
               
                
            }

           
            return true;
        }
    }
}

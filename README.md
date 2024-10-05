# Telegram Bot For Torrserver

Данный проект — бот для Telegram, который включает функции автосмены пароля [Torrserver](https://github.com/YouROK/TorrServer), а также возможность ручной смены пароля через бота.

## Установка

Для корректной работы внутри проекта необходимо создать файл `settings.ini` с следующими полями:

```ini
[Profile0]
YourBotTelegramToken=ваш токен бота
AdminChatId=ваш id чата в Telegram
TimeAutoChangePassword=время автосмены пароля, например 23:00
FilePathTorrserverBd=/opt/torrserver/accs.db   ; путь к настройкам torrserver
FilePathTor=/opt/torrserver/torrserver         ; путь к самому torrserver файлу (запускаемый,название может файла отличаться у вас)
```

## Как запустить

1. **Скачайте архив проекта**:
   - Перейдите на [эту](https://github.com/IGNATOV93/FreeTorrserverBot/releases/tag/v1.01) и скачайте последний архив.

2. **Распакуйте архив**:
   - Используйте команду:
     ```bash
     sudo unrar x FreeTorrBot-linux-64.rar /opt/
     ```

1. **Установите .NET 8**:
   - Выполните следующие команды:
     ```bash
     sudo apt-get install -y wget apt-transport-https
     wget https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb
     sudo dpkg -i packages-microsoft-prod.deb
     sudo apt-get update
     sudo apt-get install -y dotnet-sdk-8.0
     ```
   - Проверьте установку:
     ```bash
     dotnet --version
     ```
2. **Установите права на чтение и запись**:
   - Установите права на чтение для файла `FreeTorrBot` в папке бота:
     ```bash
     sudo chmod +r /opt/FreeTorrBot/FreeTorrBot
     ```
   - Установите права на чтение и запись для файла `accs.db` в папке Torrserver:
     ```bash
     sudo chmod 664 /opt/torrserver/accs.db
     ```
   - Установите права на чтение для файла `TorrServer-linux-amd64` в папке Torrserver:
     ```bash
     sudo chmod +r /opt/torrserver/TorrServer-linux-amd64
     ```

4. **Запустите проект в фоне через screen**:
   - Используйте команду:
     ```bash
     screen -S ftor && cd /opt/FreeTorrBot/ && ./FreeTorrBot
     ```

